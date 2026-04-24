using Core.BusEvents;
using UnityEngine;

namespace Core.Ads
{
    public class AdsManager
    {
        private IAdMediation _adMediation;
        private IEventBus _eventBus;

        public AdsManager(IAdMediation adMediation, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _adMediation = adMediation;
        }

        public void Init()
        {
            _adMediation.Init();
            Debug.Log("AdsManager Subscribe");
            _eventBus.Subscribe<GameResultEvent>(ShowAdOnGameOver);
        }

        ~AdsManager()
        {
            _eventBus.Unsubscribe<GameResultEvent>(ShowAdOnGameOver);
        }

        private void ShowAdOnGameOver(GameResultEvent gameResultEvent)
        {
            Debug.Log("Show ad Game Over");
            _adMediation.ShowInterstitial();
        }
    }
}