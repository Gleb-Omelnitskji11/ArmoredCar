using GameServices;
using GameUnits;
using UnityEngine;
using Zenject;

public class Turret : ObjectPool
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _turretObj;
    
    private TurretModel _turretModel;
    private BulletModel _currentBulletModel;
    private IInputProvider _input;
    private Quaternion _targetRotation = Quaternion.identity;
    private float _carSpeed;
    private float _shootTimer;
    private bool _stop = true;

    [Inject]
    private void Construct(IInputProvider input)
    {
        _input = input;
    }

    public void Init(TurretModel model, float carSpeed)
    {
        _turretModel = model;
        _carSpeed = carSpeed;
        _currentBulletModel = _turretModel.BulletPrefab[0];
        ReturnAll();
        SetPrefab(_currentBulletModel.BulletPrefab.gameObject);
    }

    public void Stop()
    {
        _stop = true;
    }

    public void Activate()
    {
        _stop = false;
    }

    private void Update()
    {
        if (_stop)
            return;
        
        HandleRotation();
        HandleShooting();
    }

    private void HandleShooting()
    {
        _shootTimer += Time.deltaTime;

        if (_shootTimer < _turretModel.FireDelay)
            return;

        Shot();
        _shootTimer = 0f;
    }

    private void Shot()
    {
        var bullet = Get();
        bullet.transform.position = _bulletSpawnPoint.position;
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.Init(this, _currentBulletModel);
        projectile.gameObject.SetActive(true);
        projectile.StartMovement(_bulletSpawnPoint.forward, _carSpeed);
    }

    private void HandleRotation()
    {
        if (!_input.TryGetInputPosition(out Vector3 targetPoint))
            return;

        Vector3 direction = targetPoint - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.01f)
            return;

        _targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            _targetRotation,
            _turretModel.RotationSpeed * Time.deltaTime
        );
    }
}