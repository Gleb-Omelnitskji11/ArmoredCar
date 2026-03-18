using ConfigData;
using Core;
using DG.Tweening;
using GameServices;
using UnityEngine;

namespace GameUnits
{
    public class Projectile : MonoBehaviour, IPooledObject
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private TrailRenderer _trailRenderer;
        
        private BulletModel _bulletModel;
        private Tweener _tween;
        public GameObject Monobehaviour => gameObject;

        public bool _inPool { get; private set; }
        public ObjectPool Pool { get; private set; }

        public void Init(ObjectPool objectPool, BulletModel bulletModel)
        {
            Pool = objectPool;
            _bulletModel = bulletModel;
            _inPool = false;
        }

        public void StartMovement(Vector3 forward)
        {
            Vector3 newPos = transform.position + forward * _bulletModel.ProjectSpeed * _bulletModel.ProjectLifetime;
            _tween = transform.DOMove(newPos, _bulletModel.ProjectLifetime).SetEase(Ease.Linear);
            _tween.OnComplete(TurnOff);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Enemy))
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
            _inPool = true;
            _tween?.Kill();
            _trailRenderer.Clear();
            Pool.ReturnToPool(this);
        }
    }
}