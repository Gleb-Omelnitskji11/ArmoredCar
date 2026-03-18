using GameServices;
using UnityEngine;

namespace GameUnits
{
    public interface IPooledObject
    {
        public bool _inPool { get;}
        public ObjectPool Pool { get;}
        public void ReturnToPool();
        public GameObject Monobehaviour { get;}
    }
}