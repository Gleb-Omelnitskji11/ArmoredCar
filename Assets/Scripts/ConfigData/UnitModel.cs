using System;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class UnitModel
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private UnitType _modelType;
        [SerializeField] private int _collisionDamage;
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private float _speed;

        public int MaxHp => _maxHp;
        public UnitType ModelType => _modelType;
        public int CollisionDamage => _collisionDamage;
        public float Speed => _speed;
        public GameObject UnitPrefab => _unitPrefab;
    }
}