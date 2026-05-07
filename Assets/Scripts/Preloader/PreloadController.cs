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
            InitPlayerPrefs();
            InitAdjust();

            if (await FirebaseInitialization.InitFirebase())
            {
                StartCoroutine(GoToGameCoroutine());
            }
            else Debug.LogError($"{nameof(FirebaseInitialization)} failed");
        }

        private IEnumerator GoToGameCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(Constants.GameScene);
        }

        private void InitPlayerPrefs()
        {
            _playerProgressSaver.Initialize();
        }

        private void InitAdjust()
        {
            //AdjustInitialization.Init();
        }

        private void InitAd()
        {
            _adsManager.Init();
        }
    }
}