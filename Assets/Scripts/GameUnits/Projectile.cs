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
            _trailRenderer.Clear();
            _tween?.Kill();
            Vector3 newPos = transform.position + forward * _bulletModel.ProjectSpeed * _bulletModel.ProjectLifetime;
            _tween = transform.DOMove(newPos, _bulletModel.ProjectLifetime).SetEase(Ease.Linear);
            _tween.OnComplete(ReturnToPool);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Enemy))
            {
                if (other.TryGetComponent<BasicEnemy>(out BasicEnemy enemy))
                {
                    enemy.TakeDamage(_bulletModel.DamageShoot);
                }

                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            _inPool = true;
            _tween?.Kill();
            Pool.ReturnToPool(this);
        }

        public void OnPausedChanged(bool isPaused)
        {
            if(isPaused) Pause();
            else Resume();
        }
        
        public void Pause()
        {
            if (_inPool) return;
            _tween?.Pause();
        }

        public void Resume()
        {
            if (_inPool) return;
            _tween?.Play();
        }
    }
}