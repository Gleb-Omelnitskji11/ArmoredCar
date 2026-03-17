using System;
using ConfigData;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _timerText;
        private LevelModel _level;
        private float _startZ;
        private int _progress;
        private Transform _carTransform;
        private bool _paused = true;

        public void Setup(LevelModel level, float startZ, Transform carTransform)
        {
            _carTransform = carTransform;
            _startZ = startZ;
            _level = level;
            _paused = false;
        }

        private void Update()
        {
            if (_paused) return;
            float progress = ((_carTransform.position.z - _startZ) / (_level.Distance - _startZ)) * 100;
            _progressBar.value = progress;
            if (_progress != (int)progress)
            {
                _progress = (int)progress;
                _timerText.text = _progress + "%";
            }

            if (_progress >= 100)
            {
                _paused = true;
                OnFinish?.Invoke();
            }
        }

        public event Action OnFinish;
    }
}