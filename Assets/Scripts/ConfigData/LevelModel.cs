using System;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public struct LevelModel
{
    [SerializeField] private float _distance;
    [SerializeField] private UnitType[] _enemyTypes;
    
    public float Distance => _distance;
    public ReadOnlyCollection<UnitType> EnemyTypes => Array.AsReadOnly(_enemyTypes);
}