using System;
using GameUnits;
using UnityEngine;

namespace ConfigData
{
    [Serializable]
    public class EnemyUnitModel
    {
        [SerializeField] private BasicEnemy _enemyPrefab;
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private UnitModel _unitModel;
        
        public BasicEnemy EnemyPrefab => _enemyPrefab;
        public EnemyType EnemyType => _enemyType;
        public UnitModel UnitModel => _unitModel;
    }
}