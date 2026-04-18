using System;
using System.Collections;
using System.Threading.Tasks;
using Core.ObjectPool;
using Firebase;
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
            container.Bind<PlayerPrefsSaver>().AsSingle()
                .OnInstantiated<PlayerPrefsSaver>((ctx, foo) => foo.Initialize()).NonLazy();
            Invoke(nameof(GoToGame), 0.5f);
        }

        private async void GoToGame()
        {
            if(await InitFirebase(StaticContext.Container))
                StartCoroutine(GoToGameCoroutine());
            else Debug.LogError($"{nameof(InitFirebase)} failed");
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }
        
        private async Task<bool> InitFirebase(DiContainer container)
        {
            try
            {
                DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseApp firebaseApp = FirebaseApp.DefaultInstance;
                    container.Bind<FirebaseApp>().FromInstance(firebaseApp);

                    Debug.Log("Firebase init success!");
                    return true;
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception during trying init firebase: {ex}");
            }

            return false;
        }
    }
}