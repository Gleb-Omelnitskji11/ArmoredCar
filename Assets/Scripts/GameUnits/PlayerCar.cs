using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerCar : Unit
{
    [SerializeField] private Turret _turret;
    [SerializeField] private Transform[] _wheels;

    private IEnumerator _moveCoroutine;
    private float _moveSpeed;

    private Vector3 _carPosition;
    private Sequence _seq;

#if UNITY_EDITOR
    //test 
    [SerializeField] private bool _stop;
    private bool _isMoving = true;

    private void Update()
    {
        if (_isMoving && _stop)
        {
            _isMoving = false;
            _seq?.Kill();
        }

        if (!_isMoving && !_stop)
        {
            UpdateDirection();
            _isMoving = true;
        }
    }
#endif
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
        _seq?.Kill();
        UpdateDirection();
        _turret.Activate();
    }

    private void UpdateDirection()
    {
        float newZ = transform.position.z + 9999f;
        float duration = 9999f / _moveSpeed;
        _seq = DOTween.Sequence();
        _seq.Join(transform.DOMoveZ(newZ, duration))
            .SetEase(Ease.Linear);
        foreach (var wheel in _wheels)
        {
            _seq.Join(wheel.DORotate(new Vector3(360f, 0f, 0f), 2f, RotateMode.FastBeyond360));
        }
        _seq.OnComplete(UpdateDirection);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constantns.Enemy))
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
        _seq?.Kill();
        _turret.Stop();
    }
}