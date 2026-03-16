using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
{
    [SerializeField] private ConfigProvider _configProvider;
    
    public override void InstallBindings()
    {
        StaticContext.Container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
        GoToGame();
        //BindConfigs();
    }
    
    // private void BindConfigs()
    // {
    //     DiContainer container = StaticContext.Container;
    //     container.Bind<UnitsConfig>().FromInstance(_unitConfig).AsSingle();
    // }

    private void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}