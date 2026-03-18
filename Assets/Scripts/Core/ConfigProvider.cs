using ConfigData;
using UnityEngine;

namespace Core
{
    public class ConfigProvider : MonoBehaviour
    {
        [SerializeField] private UnitsConfig _unitConfig;

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public UnitsConfig UnitConfig => _unitConfig;
    }
}