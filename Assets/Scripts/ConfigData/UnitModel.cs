using System;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class UnitModel
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _collisionDamage;
        [SerializeField] private float _speed;

        public int MaxHp => _maxHp;
        public int CollisionDamage => _collisionDamage;
        public float Speed => _speed;
    }

    [Serializable]
    public class EnemyUnitModel : UnitModel
    {
        [SerializeField] private EnemyType _type;
        public EnemyType EnemyType => _type;
    }
}