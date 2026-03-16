using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerCar : Unit
{
    [SerializeField] private Turret _turret;

    private IEnumerator _moveCoroutine;
    private float _moveSpeed;

    private Vector3 _carPosition;
    private Tween _tween;
    
    //test
    [SerializeField] private bool _stop;
    private bool _isMoving = true;

    private void Update()
    {
        if (_isMoving && _stop)
        {
            _isMoving = false;
            _tween?.Kill();
        }

        if (!_isMoving && !_stop)
        {
            UpdateDirection();
            _isMoving = true;
        }
    }
//
    public override void InitUnit(UnitModel model, params object[] additionalPrms)
    {
        base.InitUnit(model);
        TurretModel turretModel = additionalPrms[0] as TurretModel;
        _turret.Init(turretModel, _moveSpeed);
        _moveSpeed = model.Speed;
        _hpBar.Init(model.MaxHp);
    }

    public void StartLevel()
    {
        _tween?.Kill();
        UpdateDirection();
        _turret.StartShooting();
    }

    private void UpdateDirection()
    {
        float newZ = transform.position.z + 9999f;
        float duration = 9999f / _moveSpeed;
        _tween = transform.DOMoveZ(newZ, duration)
            .SetEase(Ease.Linear).OnComplete(UpdateDirection);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
            {
                int damageTaken = enemy.GetCollisionDamage();
                enemy.TakeDamage(GetCollisionDamage());
                TakeDamage(damageTaken);
            }
        }
    }

    public override void TakeDamage(int damageTaken)
    {
        base.TakeDamage(damageTaken);
    }

    public override void Died()
    {
        _tween?.Kill();
        _turret.StopShooting();
    }
}