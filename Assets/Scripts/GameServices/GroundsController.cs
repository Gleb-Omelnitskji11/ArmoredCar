using UnityEngine;
using Zenject;

public class GroundsController : MonoBehaviour
{
    [SerializeField] private Transform[] _grounds;

    private const float GroundStart = 0f;
    private const float MoverOffset = 75f;
    private const float ZTriggerOffset = -30f;
    
    private Transform _car;
    private int _additionalGroundIndex = 1;
    private int _currentGroundIndex;
    private float _nextZPoint;

    [Inject]
    public void Construct(PlayerCar car)
    {
        _car = car.transform;
    }

    private void Update()
    {
        if (LevelProgression.IsPaused) return;

        if (_car.position.z >= _nextZPoint)
        {
            Vector3 requiredPosition = _grounds[_currentGroundIndex].position;
            requiredPosition.z += MoverOffset;
            _grounds[_additionalGroundIndex].position = requiredPosition;

            (_additionalGroundIndex, _currentGroundIndex) = (_currentGroundIndex, _additionalGroundIndex);
            _nextZPoint += MoverOffset;
        }
    }

    public void Restart()
    {
        _grounds[0].position = new Vector3(0f, 0f, GroundStart);
        _grounds[1].position = new Vector3(0f, 0f, MoverOffset);
        _currentGroundIndex = 0;
        _additionalGroundIndex = 1;
        _nextZPoint = ZTriggerOffset;
    }
}