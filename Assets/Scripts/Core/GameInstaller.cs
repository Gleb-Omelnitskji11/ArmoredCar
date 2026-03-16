using GameServices;
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
    [SerializeField] private PlayerInputProvider _playerInputProvider;
    
    public override void InstallBindings()
    {
        Container.Bind<PlayerCar>().FromInstance(_player).AsCached();
        Container.Bind<GroundsController>().FromInstance(_groundsController).AsCached();
        Container.Bind<CameraController>().FromInstance(_cameraController).AsCached();
        Container.Bind<ResultPopup>().FromInstance(_resultPopup).AsCached();
        Container.Bind<EnemyCreator>().FromInstance(_enemyCreator).AsCached();
        Container.Bind<LevelProgression>().FromInstance(_levelProgression).AsCached();
        Container.Bind<IInputProvider>().FromInstance(_playerInputProvider).AsCached();
    }
}