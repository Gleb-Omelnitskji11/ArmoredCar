using ConfigData;
using Core;
using Core.BusEvents;
using GameUnits;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class EnemyCreator : ObjectPool
    {
        [SerializeField] private float _enemyRemoveDistance = 5f;
        [SerializeField] private Vector3 _enemyOffSet = new Vector3(2f, 4f, 16f);
        [SerializeField] private float _zEnemyMinDistance = 6f;
        [SerializeField] private float _spawnEnemyDelay = 2f;
        
        private UnitsConfig _unitsConfig;
        private PlayerCar _playerCar;
        private IEventBus _eventBus;

        private Transform _carTransform;
        private UnitModel _currentEnemyModel;
        private bool _isPaused = true;
        private float _timer;

        [Inject]
        public void Construct(ConfigProvider configProvider, PlayerCar playerCar, IEventBus eventBus)
        {
            _unitsConfig = configProvider.UnitConfig;
            _playerCar = playerCar;
            _eventBus = eventBus;
        }

        private void Start()
        {
            _currentEnemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);
            _carTransform = _playerCar.transform;
            SetPrefab(_currentEnemyModel.UnitPrefab);

            Subscribe();
        }
        
        private void Subscribe()
        {
            _eventBus.Subscribe<GameResultEvent>(OnGameResult);
            _eventBus.Subscribe<RestartEvent>(Restart);
            _eventBus.Subscribe<PauseEvent>(OnPauseResult);
        }
        
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameResultEvent>(OnGameResult);
            _eventBus.Unsubscribe<RestartEvent>(Restart);
            _eventBus.Unsubscribe<PauseEvent>(OnPauseResult);
        }

        private void Update()
        {
            if (_isPaused)
                return;

            _timer += Time.deltaTime;

            if (_timer < _spawnEnemyDelay)
                return;

            SpawnEnemies();
            _timer = 0f;
        }

        public void RefreshLevel()
        {
            ReturnAll();
            SetLevel();
            SetNearestEnemies();
            ManagePaused(false);
        }

        private void SetLevel()
        {
            initialSize = _unitsConfig.GetDefaultLevelModel.StartEnemyCount;
            _spawnEnemyDelay = _unitsConfig.GetDefaultLevelModel.EnemyDelay;
            _currentEnemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);
        }

        private void ManagePaused(bool isPaused, bool resetTimer = true)
        {
            _isPaused = isPaused;
            if (resetTimer) _timer = 0f;
        }

        private void SetNearestEnemies()
        {
            for (int i = 0; i < initialSize; i++)
            {
                SpawnNewEnemy(_carTransform.position);
            }
        }

        private void SpawnNewEnemy(Vector3 carPos)
        {
            Vector3 pos =GetRandomPosition(carPos);
            var enemy = Get(out bool isNew);
            enemy.GetComponent<ChaseEnemy>().InitUnit(_currentEnemyModel, this, _carTransform);
            enemy.transform.position = pos;
            enemy.SetActive(true);
        }

        private Vector3 GetRandomPosition(Vector3 carPos)
        {
            Vector3 pos = default;
            const int maxAttempts = 10;

            for (int i = 0; i < maxAttempts; i++)
            {
                float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
                float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
                pos = new Vector3(xRandom, _enemyOffSet.y, carPos.z + zRandom);
                if(IsPositionValid(pos))
                    break;
            }
            
            return pos;
        }

        private bool IsPositionValid(Vector3 position, float minDistance = 2.5f)
        {
            float sqrMinDistance = minDistance * minDistance;

            foreach (var enemy in AllObjects)
            {
                var go = enemy.Monobehaviour;

                if (!go.activeSelf)
                    continue;

                float sqrDistance = (go.transform.position - position).sqrMagnitude;

                if (sqrDistance < sqrMinDistance)
                    return false;
            }

            return true;
        }

        private void Resume()
        {
            ManagePaused(false, false);
        }

        private void SpawnEnemies()
        {
            Vector3 carPos = _carTransform.position;
            CheckFarEnemies(carPos);
            SpawnNewEnemy(carPos);
        }

        private void CheckFarEnemies(Vector3 carPos)
        {
            foreach (var enemy in AllObjects)
            {
                if (!enemy._inPool && enemy.Monobehaviour.transform.position.z <= carPos.z - _enemyRemoveDistance)
                {
                    enemy.Monobehaviour.GetComponent<ChaseEnemy>().TurnOff();
                }
            }
        }
        
        private void OnGameResult(GameResultEvent gameResultEvent)
        {
            Clear();
            ManagePaused(true);
        }
        
        private void OnPauseResult(PauseEvent pauseEvent)
        {
            if (pauseEvent.IsPause) 
                ManagePaused(true);
            else Resume();
        }

        private void Restart(RestartEvent restartEvent)
        {
            RefreshLevel();
        }
    }
}