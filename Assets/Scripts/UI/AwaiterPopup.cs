using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.BusEvents;
using Cysharp.Threading.Tasks;
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
        
        private float _timer;
        private Tween _timerTween;
        private CancellationTokenSource _cts;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
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

        private async UniTask StartCountdown(CancellationToken token)
        {
            for (int i = _seconds; i > 0; i--)
            {
                _timerText.text = i.ToString();
                await WaitOneSecond(token);
            }

            _timerText.text = "0";
            StartGame();
        }

        private async UniTask WaitOneSecond(CancellationToken token)
        {
            await UniTask.Delay(1000, cancellationToken: token);
        }

        private void StartGame()
        {
            gameObject.SetActive(false);
            _eventBus.Publish<RestartEvent>(new RestartEvent(true));
        }
    }
}