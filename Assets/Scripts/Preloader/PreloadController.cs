using System;
using System.Collections;
using System.Threading.Tasks;
using Core;
using Core.Ads;
using Firebase;
using GameServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Preloader
{
    public class PreloadController : MonoBehaviour
    {
        private PlayerProgressSaver _playerProgressSaver;
        private FirebaseAnalytic _firebaseAnalytic;
        private AdjustAnalytic _adjustAnalytic;
        private AdsManager _adsManager;

        [Inject]
        public void Construct(PlayerProgressSaver playerProgressSaver, FirebaseAnalytic firebaseAnalytic,
            AdjustAnalytic adjustAnalytic, AdsManager adsManager)
        {
            _adsManager = adsManager;
            _adjustAnalytic = adjustAnalytic;
            _firebaseAnalytic = firebaseAnalytic;
            _playerProgressSaver = playerProgressSaver;
        }

        private void Start()
        {
            GoToGame();
        }

        private async void GoToGame()
        {
            InitAd();
            IntPlayerPrefs();
            InitAdjust();

            if (await InitFirebase())
            {
                StartCoroutine(GoToGameCoroutine());
            }
            else Debug.LogError($"{nameof(InitFirebase)} failed");
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }

        private async Task<bool> InitFirebase()
        {
            try
            {
                DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseApp firebaseApp = FirebaseApp.DefaultInstance;

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

        private void IntPlayerPrefs()
        {
            _playerProgressSaver.Initialize();
        }

        private void InitAdjust()
        {
            _adjustAnalytic.Init();
        }

        private void InitAd()
        {
            _adsManager.Init();
        }
    }
}