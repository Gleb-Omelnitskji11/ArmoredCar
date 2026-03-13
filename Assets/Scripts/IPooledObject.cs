using UnityEngine;

public interface IPooledObject
{
    public bool _isNotInPool { get;}
    public ObjectPool Pool { get;}
    public void TurnOff();
    public GameObject Monobehaviour { get;}
}