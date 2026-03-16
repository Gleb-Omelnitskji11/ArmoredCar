using UnityEngine;
using Zenject;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private Vector3 _carStartPos = new Vector3(0, 0.5f, -48.6f);
    
    private PlayerCar _car;
    private GroundsController _groundsController;
    private ResultPopup _resultPopup;
    private EnemyCreator _enemyCreator;
    private UnitsConfig _unitsConfig;
    
    private LevelModel _levelModel;

    public static bool IsPaused { get; private set; } = true;

    [Inject]
    public void Construct(PlayerCar playerCar, GroundsController groundsController, ResultPopup resultPopup,
        EnemyCreator enemyCreator, ConfigProvider configProvider)
    {
        _car = playerCar;
        _groundsController = groundsController;
        _resultPopup = resultPopup;
        _enemyCreator = enemyCreator;
        _unitsConfig = configProvider.UnitConfig;
    }

    private void Start()
    {
        _resultPopup.OnStartClicked += Restart;
        _car.OnDied += ShowLosePopup;
    }

    private void Restart()
    {
        IsPaused = false;
        _groundsController.Restart();
        ResetCar();
        _levelModel = _unitsConfig.GetDefaultLevelModel;
        SetFinishPoint();
        _enemyCreator.RefreshLevel();
        _enemyCreator.StartSpawnDynamicEnemies();
        _car.StartLevel();
    }

    private void ResetCar()
    {
        _car.transform.position = _carStartPos;
        var carModel = _unitsConfig.GetUnitModel(UnitType.Player);
        var turretModel = _unitsConfig.GetTurretModel(0);
        _car.InitUnit(carModel, turretModel);
    }

    private void SetFinishPoint()
    {
        Vector3 curPoint = transform.position;
        curPoint.z = _levelModel.Distance + _carStartPos.z;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        IsPaused = true;
        _enemyCreator.StopSpawnDynamicEnemies();
        _resultPopup.ShowResult(true);
    }

    public void ShowLosePopup()
    {
        IsPaused = true;
        _enemyCreator.StopSpawnDynamicEnemies();
        _resultPopup.ShowResult(false);
    }
}