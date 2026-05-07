using System.Collections.Generic;
using GameServices;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Core.Ads
{
    public class AdMobMediation : IAdMediation
    {
        private const string InterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        private InterstitialAd _interstitialAd;

        public void Init()
        {
            MobileAds.Initialize((initStatus) =>
            {
                Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
                {
                    string className = keyValuePair.Key;
                    AdapterStatus status = keyValuePair.Value;
                    switch (status.InitializationState)
                    {
                        case AdapterState.NotReady:
                            Debug.LogWarning("Adapter: " + className + " not ready.");
                            break;
                        case AdapterState.Ready:
                            Debug.Log("Adapter: " + className + " is initialized.");
                            LoadInterstitial();
                            break;
                    }
                }
            });
        }

        public void LoadInterstitial()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");
            // create our request used to load the ad.
            var adRequest = new AdRequest();
            // send the request to load the ad.
            InterstitialAd.Load(InterstitialAdUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());
                    _interstitialAd = ad;
                    RegisterEventHandlers(ad);
                });
        }

        public void ShowInterstitial()
        {
            //AdjustInitialization.CheckStatus();
            FirebaseInitialization.CheckStatus();
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }

        private void RegisterEventHandlers(InterstitialAd interstitialAd)
        {
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitial();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " + "with error : " + error);
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitial();
            };
        }
    }
}