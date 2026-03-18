using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class BulletModel
    {
        [SerializeField] private int _damageShoot;
        [SerializeField] private float _projectSpeed;
        [SerializeField] private float _projectLifetime;
        [SerializeField] private Projectile _bulletPrefab;

        public int DamageShoot => _damageShoot;
        public float ProjectSpeed => _projectSpeed;
        public float ProjectLifetime => _projectLifetime;

        public Projectile BulletPrefab => _bulletPrefab;
    }
}