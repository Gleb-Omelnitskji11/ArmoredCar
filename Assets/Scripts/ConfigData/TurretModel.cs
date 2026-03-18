using System;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class TurretModel
    {
        [SerializeField] private float _fireDelay;
        [SerializeField] private float _damageShoot;

        [SerializeField] private float _rotationSpeed;

        [SerializeField] private BulletModel[] _bulletModels;
        [SerializeField] private int _turretId;

        public float FireDelay => _fireDelay;

        public float RotationSpeed => _rotationSpeed;
        public BulletModel[] BulletPrefab => _bulletModels;
        public int TurretId => _turretId;
    }
}