using System.Collections;
using UnityEngine;

public class EnemyCreator : ObjectPool
{
    [SerializeField] private UnitsConfig _unitsConfig;
    [SerializeField] private PlayerCar _playerCar;
    
    [SerializeField] private float _enemyRemoveDistance = 5f;
    [SerializeField] private Vector3 _enemyOffSet = new Vector3(2f, 4f, 16f);
    [SerializeField] private float _zEnemyMinDistance = 6f;
    [SerializeField] private float _spawnEnemyDelay = 2f;

    private Transform _carTransform;
    private IEnumerator _dynamicEnemySpawner;
    private UnitModel _currentEnemyModel;

    private void Start()
    {
        _dynamicEnemySpawner = SpawnEnemies();
        _currentEnemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);
        SetPrefab(_unitsConfig.GetUnitModel(UnitType.Enemy1).UnitPrefab);
        CreateStartObjects();
    }

    public void RefreshLevel()
    {
        ReturnAll();
        var carModel = _unitsConfig.GetUnitModel(UnitType.Player);
        var turretModel = _unitsConfig.GetTurretModel(0);
        _playerCar.InitUnit(carModel, turretModel);
        _carTransform = _playerCar.transform;
        var enemyModel = _unitsConfig.GetUnitModel(UnitType.Enemy1);
        
        for (int i = 0; i < _unitsConfig.EnemyStartCount; i++)
        {
            float zRandom = Random.Range(_zEnemyMinDistance, _enemyOffSet.z);
            zRandom += _carTransform.position.z;
            float xRandom = Random.Range(-_enemyOffSet.x, _enemyOffSet.x);
            var enemy = Get();
            enemy.transform.position = new Vector3(xRandom, _enemyOffSet.y, zRandom);
            enemy.GetComponent<BasicEnemy>().InitUnit(enemyModel, this);
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
