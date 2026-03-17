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

        private Transform _carTransform;
        private UnitModel _currentEnemyModel;
        private bool _isPaused = true;
        private float _timer;

        [Inject]
        public void Construct(ConfigProvider configProvider, PlayerCar playerCar)
        {
            _unitsConfig = configProvider.UnitConfig;
            _playerCar = playerCar;
        }

        private void Start()
        {
            _currentEnemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);
            _carTransform = _playerCar.transform;
            SetPrefab(_currentEnemyModel.UnitPrefab);
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
            float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
            float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
            Vector3 pos = new Vector3(xRandom, _enemyOffSet.y, carPos.z + zRandom);
            var enemy = Get();
            enemy.GetComponent<ChaseEnemy>().InitUnit(_currentEnemyModel, this, _carTransform);
            enemy.transform.position = pos;
            enemy.SetActive(true);
        }

        public void Stop()
        {
            ManagePaused(false);
        }

        public void Resume()
        {
            ManagePaused(true, false);
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
                if (enemy.Monobehaviour.transform.position.z <= carPos.z - _enemyRemoveDistance)
                {
                    enemy.Monobehaviour.GetComponent<ChaseEnemy>().TurnOff();
                }
            }
        }
    }
}