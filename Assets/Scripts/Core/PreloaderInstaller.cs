using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class PreloaderInstaller : MonoBehaviour
    {
        [SerializeField] private ConfigProvider _configProvider;
    
        private void Start()
        {
            StaticContext.Container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
            StaticContext.Container.Bind<IEventBus>().To<EventBus>().AsSingle();
            GoToGame();
        }

        private void GoToGame()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }
    }
}