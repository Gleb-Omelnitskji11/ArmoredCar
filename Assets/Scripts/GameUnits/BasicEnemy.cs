using ConfigData;
using Core;
using GameServices;
using UnityEngine;

namespace GameUnits
{
    public class BasicEnemy : Unit, IPooledObject
    {
        public GameObject Monobehaviour => gameObject;
        public bool _inPool { get; protected set; }
        public ObjectPool Pool { get; protected set; }
        public void InitUnit(UnitModel model, ObjectPool objectPool)
        {
            base.InitUnit(model);
        
            Pool = objectPool;
            _inPool = true;
        }

        public override void Died()
        {
            TurnOff();
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

        public void TurnOff()
        {
            _inPool = true;
            Pool.ReturnToPool(this);
        }
    }
}