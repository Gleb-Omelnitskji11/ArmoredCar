using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Slider _hpBar;
        [SerializeField] private Image _fillerImage;
        [SerializeField] private Image _bgImage;
        [SerializeField] private HpState[] _hpStates;
    
        public void Init(int max)
        {
            _hpBar.maxValue = max;
            UpdateState(max);
        }

        public void UpdateState(int newValue)
        {
            _hpBar.value = newValue;
            int i = 0;
            for (; i < _hpStates.Length; i++)
            {
                if (_hpStates[i].MinValue > (float) newValue/ _hpBar.maxValue)
                    break;
            }

            _fillerImage.color = _hpStates[i - 1].Color;
        }

        [Serializable]
        public class HpState
        {
            public Color Color;
            public float MinValue;
        }
    }
}
