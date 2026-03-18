using ConfigData;
using GameServices;
using UnityEngine;

namespace GameUnits
{
    public class ChaseEnemy : BasicEnemy
    {
        private static readonly int Chase = Animator.StringToHash("Chase");

        [SerializeField] private float _agroDistance = 10f;
        [SerializeField] private float _moveSpeed = 4f;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _rotateTransform;

        private Transform _player;
        private bool _isChasing;

        public void InitUnit(UnitModel model, ObjectPool objectPool, Transform carTransform)
        {
            base.InitUnit(model, objectPool);

            _player = carTransform;
            _isChasing = false;
            SetIdle();
        }

        private void Update()
        {
            float sqrDistance = (_player.position - transform.position).sqrMagnitude;

            if (!_isChasing && sqrDistance <= _agroDistance * _agroDistance)
            {
                StartChase();
            }

            if (_isChasing)
            {
                MoveToPlayer();
            }
        }

        private void StartChase()
        {
            _isChasing = true;
            _animator.SetBool(Chase, true);
        }

        private void SetIdle()
        {
            _isChasing = false;
            _animator.SetBool(Chase, false);
        }

        private void MoveToPlayer()
        {
            Vector3 direction = _player.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude < 0.01f)
                return;

            direction.Normalize();

            transform.position += direction * _moveSpeed * Time.deltaTime;

            Vector3 localDirection = transform.InverseTransformDirection(direction);
            _rotateTransform.localRotation = Quaternion.LookRotation(localDirection);
        }

        public override void Died()
        {
            SetIdle();
            base.Died();
        }
    }
}
