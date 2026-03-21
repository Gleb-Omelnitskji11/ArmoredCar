using ConfigData;
using Core;
using Core.ObjectPool;
using UnityEngine;

namespace GameUnits
{
    public class BasicEnemy : Unit, IPooledObject
    {
        private const float EnemyRemoveDistance = 5f;
        protected Transform Player;
        private EnemyUnitModel _enemyUnitModel;

        public string PoolKey { get; protected set; }
        public bool IsActive { get; protected set; }
        public IObjectPooler Pooler { get; protected set; }
        public EnemyType EnemyType => _enemyUnitModel.EnemyType;

        public virtual void InitEnemyModel(EnemyUnitModel model, Transform carTransform)
        {
            InitUnit(model.UnitModel);
            _enemyUnitModel = model;
            Player = carTransform;
        }

        protected override void Died()
        {
            Deactivate();
        }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            if (!IsActive) return;
            if (IsFar()) Deactivate();
        }

        protected bool IsFar()
        {
            return transform.position.z <= Player.position.z - EnemyRemoveDistance;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Player))
            {
                if (other.TryGetComponent<PlayerCar>(out PlayerCar player))
                {
                    TakeLethalDamage();
                    player.TakeDamage(GetCollisionDamage());
                }
            }
        }

        public void SetPoolData(IObjectPooler pooler, string poolKey)
        {
            PoolKey = poolKey;
            Pooler = pooler;
        }

        public void Deactivate()
        {
            IsActive = false;
            Pooler.Release(PoolKey, this);
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            IsActive = true;
            Reset();
        }
    }
}