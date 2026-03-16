using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyCreator _enemyCreator;
    [SerializeField] private GroundsController _groundsController;
    [SerializeField] private PlayerCar _player;
    [SerializeField] private LevelProgression _levelProgression;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private ResultPopup _resultPopup;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerCar>().FromInstance(_player);
        Container.Bind<GroundsController>().FromInstance(_groundsController);
        Container.Bind<CameraController>().FromInstance(_cameraController);
        Container.Bind<ResultPopup>().FromInstance(_resultPopup);
        Container.Bind<EnemyCreator>().FromInstance(_enemyCreator);
        Container.Bind<LevelProgression>().FromInstance(_levelProgression);
    }
}