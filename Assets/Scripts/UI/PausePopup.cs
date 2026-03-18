using Core;
using Core.BusEvents;
using GameServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PausePopup : MonoBehaviour
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        private LevelLoader _levelLoader;
        private IEventBus _eventBus;


        [Inject]
        public void Construct(LevelLoader levelLoader, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _levelLoader = levelLoader;
        }

        private void Start()
        {
            _resumeButton.onClick.AddListener(Resume);
            _restartButton.onClick.AddListener(Restart);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Pause();
        }

        private void Pause()
        {
            _eventBus.Publish<PauseEvent>(new PauseEvent(true));
        }

        private void Resume()
        {
            gameObject.SetActive(false);
            _eventBus.Publish<PauseEvent>(new PauseEvent(false));
        }
        
        private void Restart()
        {
            gameObject.SetActive(false);
            _eventBus.Publish<RestartEvent>(new RestartEvent());
        }
    }
}
