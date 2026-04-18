using Core;
using Core.BusEvents;
using GameServices;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class DestroyedCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        
        private IEventBus _eventBus;
        private int _counter;
        private AchieveAnalytic _achieveAnalytic;
        private const string Format = "{0} destroyed";

        [Inject]
        public void Construct(IEventBus eventBus, AchieveAnalytic achieveAnalytic)
        {
            _achieveAnalytic = achieveAnalytic;
            _eventBus = eventBus;
        }

        private void Start()
        {
            _eventBus.Subscribe<RestartEvent>(OnRestart);
            _eventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<RestartEvent>(OnRestart);
            _eventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        private void OnRestart(RestartEvent restartEvent)
        {
            _counter = 0;
            UpdateText();
        }

        private void OnEnemyDied(EnemyDiedEvent enemyDiedEvent)
        {
            _counter += enemyDiedEvent.EnemyModel.Points;
            UpdateText();
            _achieveAnalytic.AddEnemyDied(_counter);
        }

        private void UpdateText()
        {
            _counterText.text = string.Format(Format, _counter);
        }
    }
}