using System.Collections;
using Core.ObjectPool;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.Installer
{
    public class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
    {
        [SerializeField] private ConfigProvider _configProvider;
        public override void InstallBindings()
        {
            DiContainer container = StaticContext.Container;
            container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
            container.Bind<IEventBus>().To<EventBus>().AsSingle();
            container.Bind<IObjectPooler>().To<ObjectPooler>().AsSingle();
            Invoke(nameof(GoToGame), 0.5f);
        }

        private void GoToGame()
        {
            StartCoroutine(GoToGameCoroutine());
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }
    }
}