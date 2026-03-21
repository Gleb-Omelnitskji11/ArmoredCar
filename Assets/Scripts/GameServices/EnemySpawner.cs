using System;
using ConfigData;
using Core;
using Core.BusEvents;
using Core.ObjectPool;
using GameUnits;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameServices
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 _enemyOffSet = new Vector3(2f, 4f, 16f);
        [SerializeField] private float _zEnemyMinDistance = 6f;
        [SerializeField] private float _spawnEnemyDelay = 2f;
        
        private GameConfig _gameConfig;
        private PlayerCar _playerCar;
        private IEventBus _eventBus;

        private bool _isPaused = true;
        private float _timer;
        private LevelModel _levelModel;
        private IObjectPooler _pooler;
        private int _initialSize;

        [Inject]
        public void Construct(IObjectPooler pooler, ConfigProvider configProvider, PlayerCar playerCar, IEventBus eventBus)
        {
            _pooler = pooler;
            _gameConfig = configProvider.GameConfig;
            _playerCar = playerCar;
            _eventBus = eventBus;
        }

        private void Start()
        {
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
            
            SpawnNewEnemy();
            _timer = 0f;
        }

        private void StartGame()
        {
            SetLevel();
            InitPools();
            SetNearestEnemies();
            ManagePaused(false);
        }
        
        private void RealiseAll()
        {
            foreach (EnemyType type in _levelModel.EnemyTypes)
            {
                string key = GetKey(type);
                _pooler.Clear(key);
            }
        }
        
        private void InitPools()
        {
            foreach (EnemyType type in _levelModel.EnemyTypes)
            {
                EnemyUnitModel model = _gameConfig.GetEnemyUnitModel(type);
                string key = GetKey(type);

                _pooler.CreatePool(key, model.EnemyPrefab, factory: CreateNewEnemyObj,
                    onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: 0);
            }
        }
        
        private void OnGetFromPool(BasicEnemy enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.Reset();
        }

        private void OnRealiseToPool(BasicEnemy enemy)
        {
            
        }
        
        private string GetKey(EnemyType type) => $"Enemy_{type}";

        private void SetLevel()
        {
            _initialSize = _gameConfig.GetDefaultLevelModel.StartEnemyCount;
            _spawnEnemyDelay = _gameConfig.GetDefaultLevelModel.EnemyDelay;
            _levelModel = _gameConfig.GetDefaultLevelModel;
        }

        private void ManagePaused(bool isPaused, bool resetTimer = true)
        {
            _isPaused = isPaused;
            if (resetTimer) _timer = 0f;
        }

        private void SetNearestEnemies()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                SpawnNewEnemy();
            }
        }

        private BasicEnemy CreateNewEnemyObj(BasicEnemy prefab)
        {
            BasicEnemy enemy = Instantiate(prefab);
            EnemyType enemyType = prefab.EnemyType;
            string key = GetKey(enemyType);
            EnemyUnitModel unitModel = _gameConfig.GetEnemyUnitModel(enemyType);
            enemy.SetPoolData(_pooler, key);
            enemy.InitEnemyModel(unitModel, _playerCar.transform);
            return enemy;
        }

        private void SpawnNewEnemy()
        {
            EnemyType enemyType = GetRandomEnemyType();
            Vector3 pos = GetRandomPosition();
            string key = GetKey(enemyType);
            var enemy = _pooler.Get<BasicEnemy>(key);
            enemy.transform.position = pos;
        }

        private EnemyType GetRandomEnemyType()
        {
            int max = _levelModel.EnemyTypes.Count;
            int random = Random.Range(0, max);
            return _levelModel.EnemyTypes[random];
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 carPos = _playerCar.transform.position;
            float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
            float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
            Vector3 pos = new Vector3(xRandom, _enemyOffSet.y, carPos.z + zRandom);
            return pos;
        }

        private void Resume()
        {
            ManagePaused(false, false);
        }
        
        private void OnGameResult(GameResultEvent gameResultEvent)
        {
            ManagePaused(true);
            foreach (var enemyType in _levelModel.EnemyTypes)
            {
                string key = GetKey(enemyType);
                _pooler.Clear(key);
            }
        }
        
        private void OnPauseResult(PauseEvent pauseEvent)
        {
            if (pauseEvent.IsPause) 
                ManagePaused(true);
            else Resume();
        }

        private void Restart(RestartEvent restartEvent)
        {
            if(!restartEvent.IsFirstGame) RealiseAll();
            StartGame();
        }
    }
}