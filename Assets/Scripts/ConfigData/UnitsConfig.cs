using System;
using ConfigData;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsConfig", menuName = "ScriptableObjects/UnitsConfig", order = 0)]
public class UnitsConfig : ScriptableObject
{
    [SerializeField] private UnitModel[] _unitModels = new UnitModel[2];
    [SerializeField] private TurretModel[] _turretModels = new TurretModel[1];
    [SerializeField] private LevelModel _defaultLevelModel;
    
    public LevelModel GetDefaultLevelModel => _defaultLevelModel;

    public UnitModel GetUnitModel(UnitType unitType)
    {
        foreach (var unit in _unitModels)
        {
            if (unit.ModelType == unitType)
                return unit;
        }
        
        throw new Exception("No unit model found");
    }
    
    public TurretModel GetTurretModel(int id)
    {
        foreach (var turret in _turretModels)
        {
            if (turret.TurretId == id)
                return turret;
        }
        
        throw new Exception("No turret model found");
    }
}