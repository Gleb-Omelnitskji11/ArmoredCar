using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using GameServices;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class AwaiterPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private int _seconds;
        private LevelLoader _levelLoader;
        private float _timer;
        private Tween _timerTween;
        private CancellationTokenSource _cts;

        [Inject]
        public void Construct(LevelLoader levelLoader)
        {
            _levelLoader = levelLoader;
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            StartCountdown(_cts.Token);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async Task StartCountdown(CancellationToken token)
        {
            for (int i = _seconds; i > 0; i--)
            {
                _timerText.text = i.ToString();
                await WaitOneSecond(token);
            }

            _timerText.text = "0";
            StartGame();
        }

        private async Task WaitOneSecond(CancellationToken token)
        {
            await Task.Delay(1000, cancellationToken: token);
        }

        private void StartGame()
        {
            gameObject.SetActive(false);
            _levelLoader.Restart();
        }
    }
}