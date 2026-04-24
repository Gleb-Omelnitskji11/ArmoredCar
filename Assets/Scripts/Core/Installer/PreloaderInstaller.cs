using Core.Ads;
using Core.ObjectPool;
using GameServices;
using UnityEngine;
using Zenject;

namespace Core.Installer
{
    public class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
    {
        [SerializeField] private ConfigProvider _configProvider;
        [SerializeField] private AdjustAnalytic _adjustAnalytic;

        public override void InstallBindings()
        {
            DiContainer container = StaticContext.Container;
            container.Bind<ConfigProvider>().FromInstance(_configProvider).AsSingle();
            container.Bind<IEventBus>().To<EventBus>().AsSingle();
            container.Bind<IObjectPooler>().To<ObjectPooler>().AsSingle();
            container.Bind<PlayerProgressSaver>().AsSingle().NonLazy();
            container.Bind<FirebaseAnalytic>().AsSingle().NonLazy();
            container.Bind<AdjustAnalytic>().FromInstance(_adjustAnalytic).AsSingle();
            container.Bind<AnalyticsManager>().AsSingle();
            container.Bind<IAdMediation>().To<AdMobMediation>().AsSingle();
            container.BindInterfacesAndSelfTo<AdsManager>().AsSingle().NonLazy();
        }
    }
}