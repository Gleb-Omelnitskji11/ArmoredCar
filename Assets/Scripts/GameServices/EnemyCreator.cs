using System.Collections;
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
            SetNearestEnemies();
            ManagePaused(false);
        }

        private void ManagePaused(bool isPaused, bool resetTimer = true)
        {
            _isPaused = isPaused;
            if (resetTimer) _timer = 0f;
        }

        private void SetNearestEnemies()
        {
            var enemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);

            for (int i = 0; i < _unitsConfig.EnemyStartCount; i++)
            {
                float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
                zRandom += _carTransform.position.z;
                float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
                var enemy = Get();
                enemy.transform.position = new Vector3(xRandom, _enemyOffSet.y, zRandom);
                enemy.GetComponent<BasicEnemy>().InitUnit(enemyModel, this);
                enemy.SetActive(true);
            }
        }

        public void Stop()
        {
            ManagePaused(false);
        }

        private void SpawnEnemies()
        {
            Vector3 carPos = _carTransform.position;
            CheckFarEnemies(carPos);
            SpawnNewEnemy(carPos);
        }

        private void SpawnNewEnemy(Vector3 carPos)
        {
            var enemy = Get();

            float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
            Vector3 pos = new Vector3(xRandom, _enemyOffSet.y, carPos.z + _enemyOffSet.z);
            enemy.GetComponent<BasicEnemy>().InitUnit(_currentEnemyModel, this);
            enemy.transform.position = pos;
            enemy.SetActive(true);
        }

        private void CheckFarEnemies(Vector3 carPos)
        {
            foreach (var enemy in AllObjects)
            {
                if (enemy.Monobehaviour.transform.position.z <= carPos.z - _enemyRemoveDistance)
                {
                    enemy.Monobehaviour.GetComponent<BasicEnemy>().TurnOff();
                }
            }
        }
    }
}