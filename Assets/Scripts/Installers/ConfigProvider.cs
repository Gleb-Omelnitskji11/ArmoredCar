using UnityEngine;
using Zenject;

public class ConfigProvider : MonoBehaviour
{
    [SerializeField] private UnitsConfig _unitConfig;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public UnitsConfig UnitConfig => _unitConfig;
}