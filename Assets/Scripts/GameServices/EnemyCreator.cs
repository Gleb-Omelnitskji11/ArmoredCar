using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyCreator : ObjectPool
{
    [SerializeField] private float _enemyRemoveDistance = 5f;
    [SerializeField] private Vector3 _enemyOffSet = new Vector3(2f, 4f, 16f);
    [SerializeField] private float _zEnemyMinDistance = 6f;
    [SerializeField] private float _spawnEnemyDelay = 2f;
    
    private UnitsConfig _unitsConfig;
    private PlayerCar _playerCar;

    private Transform _carTransform;
    private IEnumerator _dynamicEnemySpawner;
    private UnitModel _currentEnemyModel;

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
        _dynamicEnemySpawner = SpawnEnemies();
    }

    public void RefreshLevel()
    {
        ReturnAll();
        SetNearestEnemies();
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

    public void StartSpawnDynamicEnemies()
    {
        StartCoroutine(_dynamicEnemySpawner);
    }

    public void StopSpawnDynamicEnemies()
    {
        StopCoroutine(_dynamicEnemySpawner);
    }

    private IEnumerator SpawnEnemies()
    {
        while (!LevelProgression.IsPaused)
        {
            yield return new WaitForSeconds(_spawnEnemyDelay);

            Vector3 carPos = _carTransform.position;
            CheckFarEnemies(carPos);
            SpawnNewEnemy(carPos);
        }
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