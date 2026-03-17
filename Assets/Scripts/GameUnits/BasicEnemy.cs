using TMPro;
using UnityEngine;

public class BasicEnemy : Unit, IPooledObject
{
    public GameObject Monobehaviour => gameObject;
    public bool _inPool { get; protected set; }
    public ObjectPool Pool { get; protected set; }
    public override void InitUnit(UnitModel model, params object[] additionalPrms)
    {
        base.InitUnit(model);
        
        Pool = additionalPrms[0] as ObjectPool;
        _inPool = true;
    }

    public override void Died()
    {
        TurnOff();
    }
    
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constantns.Player))
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