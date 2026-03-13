using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool _follow = true;
    [SerializeField] private Transform _car;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Vector3 _gamePositionOffSet = new Vector3(0f, 10.13f, -8.14f);
    private Vector3 _currentPosition;


    //private const int _zСameraOffset = 5;
    //private const float _zCameraStart = -51f;

    private void LateUpdate()
    {
        if (_follow)
        {
            _currentPosition = _car.position + _gamePositionOffSet;
            _cameraTransform.position = _currentPosition;
        }
    }
}