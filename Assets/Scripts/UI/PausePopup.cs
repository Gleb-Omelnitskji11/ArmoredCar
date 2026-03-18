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


        [Inject]
        public void Construct(LevelLoader levelLoader)
        {
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
            _levelLoader.Pause();
        }

        private void Resume()
        {
            gameObject.SetActive(false);
            _levelLoader.Resume();
        }
        
        private void Restart()
        {
            gameObject.SetActive(false);
            _levelLoader.Restart();
        }
    }
}
