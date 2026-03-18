using GameServices;
using GameUnits;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private EnemyCreator _enemyCreator;
        [SerializeField] private GroundsController _groundsController;
        [SerializeField] private PlayerCar _player;
        [FormerlySerializedAs("_levelProgression")] [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ResultPopup _resultPopup;
        [SerializeField] private PlayerInputProvider _playerInputProvider;
        [FormerlySerializedAs("_progresBar")] [SerializeField] private ProgressBar _progressBar;
    
        public override void InstallBindings()
        {
            Container.Bind<PlayerCar>().FromInstance(_player).AsCached();
            Container.Bind<GroundsController>().FromInstance(_groundsController).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsCached();
            Container.Bind<ResultPopup>().FromInstance(_resultPopup).AsCached();
            Container.Bind<EnemyCreator>().FromInstance(_enemyCreator).AsCached();
            Container.Bind<LevelLoader>().FromInstance(_levelLoader).AsCached();
            Container.Bind<IInputProvider>().FromInstance(_playerInputProvider).AsCached();
            Container.Bind<ProgressBar>().FromInstance(_progressBar).AsCached();
        }
    }
}