using System.Collections;
using UnityEngine;

public class Turret : ObjectPool
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Transform _turretObj;
    public TurretModel TurretModel { get; private set; }

    private BulletModel _currentBulletModel;
    private IEnumerator _shootingCoroutine;
    private IEnumerator _rotatingCoroutine;
    private Camera _camera;
    private Quaternion _targetRotation = Quaternion.identity;
    private float _carSpeed;

    public void Init(TurretModel model, float carSpeed)
    {
        _camera = Camera.main;
        TurretModel = model;
        _carSpeed = carSpeed;
        _currentBulletModel = TurretModel.BulletPrefab[0];
        ReturnAll();
        SetPrefab(_currentBulletModel.BulletPrefab.gameObject);
        _shootingCoroutine = ShootingCoroutine();
        _rotatingCoroutine = RotateCoroutine();
        CreateStartObjects();
    }

    public void StartShooting()
    {
        StartCoroutine(_shootingCoroutine);
        StartCoroutine(_rotatingCoroutine);
    }

    public void StopShooting()
    {
        StopCoroutine(_shootingCoroutine);
        StopCoroutine(_rotatingCoroutine);

        foreach (var obj in AllObjects)
        {
            obj.TurnOff();
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            Shot();
            yield return new WaitForSeconds(TurretModel.FireDelay);
        }
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            HandleRotation();
            yield return null;
        }
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
        Vector3? targetPoint = GetInputPosition();

        if (targetPoint != null)
        {
            Vector3 direction = targetPoint.Value - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                _targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    _targetRotation,
                    TurretModel.RotationSpeed * Time.deltaTime
                );
            }
        }
    }

    private Vector3? GetInputPosition()
    {
        Vector3 screenPos = Vector3.zero;

        if (Input.touchCount > 0)
        {
            screenPos = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            screenPos = Input.mousePosition;
        }
        else
        {
            return null;
        }

        Ray ray = _camera.ScreenPointToRay(screenPos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return null;
    }
}