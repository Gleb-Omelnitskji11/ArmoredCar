using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
{
    [SerializeField] private ConfigProvider _configProvider;
    
    public override void InstallBindings()
    {
        StaticContext.Container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
        StaticContext.Container.Bind<IEventBus>().To<EventBus>().AsSingle();
        GoToGame();
    }

    private void GoToGame()
    {
        SceneManager.LoadScene(Constantns.GameScene);
    }
}