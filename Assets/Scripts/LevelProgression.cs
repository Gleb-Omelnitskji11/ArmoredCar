using UnityEngine;
using UnityEngine.Serialization;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private CameraController _camera;
    [SerializeField] private PlayerCar _car;
    [SerializeField] private Transform[] _grounds;
    [SerializeField] private ResultPopup _resultPopup;
    
    [SerializeField] private EnemyCreator _enemyCreator;
    [SerializeField] private Vector3 _carStartPos = new Vector3(0, 0.5f, -48.6f);
    
    private const float GroundStart = 0f;
    private const float MoverOffset = 75f;
    private int _additionalGroundIndex;
    private int _currentGroundIndex;
    
    private const int _zСameraOffset = 5;
    private const float _zCameraStart = -51f;
    
    private float _zCameraNext = 7f;
    private Vector3 _cameraPosition;

    public static bool IsPaused { get; private set; } = true;

    private void Start()
    {
        _resultPopup.OnStartClicked += Restart;
        _car.GetComponent<PlayerCar>().OnDied += ShowLosePopup;
    }

    private void Update()
    {
        if (IsPaused) return;

        //moveGround if required
        if (_cameraPosition.z >= _zCameraNext)
        {
            Vector3 requiredPosition = _grounds[_currentGroundIndex].position;
            requiredPosition.z += MoverOffset;
            _grounds[_additionalGroundIndex].position = requiredPosition;

            (_additionalGroundIndex, _currentGroundIndex) = (_currentGroundIndex, _additionalGroundIndex);
            _zCameraNext += MoverOffset;
        }
    }

    public void StopGame()
    {
        IsPaused = true;
    }

    private void Restart()
    {
        _grounds[0].position = new Vector3(0f, 0f, GroundStart);
        _currentGroundIndex = 0;
        _additionalGroundIndex = 1;
        _zCameraNext = _zCameraStart + _zСameraOffset;
        IsPaused = false;
        _car.transform.position = _carStartPos;
        _enemyCreator.RefreshLevel();
        _enemyCreator.StartSpawnDynamicEnemies();
        _car.StartGame();
    }

    private void ShowLosePopup()
    {
        IsPaused = true;
        _enemyCreator.StopSpawnDynamicEnemies();
        _resultPopup.ShowResult(false);
    }
}
