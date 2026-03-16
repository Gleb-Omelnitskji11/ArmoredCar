using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private Rigidbody _rigidbody;
    private BulletModel _bulletModel;
    private Tweener _tween;
    public GameObject Monobehaviour => gameObject;

    public bool _isNotInPool { get; private set; }
    public ObjectPool Pool { get; private set; }

    public void Init(ObjectPool objectPool, BulletModel bulletModel)
    {
        Pool = objectPool;
        _bulletModel = bulletModel;
        _isNotInPool = false;
    }

    public void StartMovement(Vector3 forward, float carSpeed)
    {
        Vector3 newPos = transform.position + forward * _bulletModel.ProjectSpeed * _bulletModel.ProjectLifetime;
        newPos.z += carSpeed * _bulletModel.ProjectLifetime;
        _tween = transform.DOMove(newPos, _bulletModel.ProjectLifetime).SetEase(Ease.Linear);
        _tween.OnComplete(TurnOff);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
            {
                enemy.TakeDamage(_bulletModel.DamageShoot);
            }

            TurnOff();
        }
    }

    public void TurnOff()
    {
        _tween?.Kill();
        Pool.ReturnToPool(this);
    }
}