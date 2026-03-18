using System;
using ConfigData;
using UI;
using UnityEngine;

namespace GameUnits
{
    public abstract class Unit : MonoBehaviour
    {
        [SerializeField] protected Transform _unitTransform;
        [SerializeField] protected int _currentHp;
        [SerializeField] protected HpBar _hpBar;
    
        protected UnitModel _unitModel;

        public virtual void InitUnit(UnitModel model)
        {
            _unitModel = model;
            _currentHp = _unitModel.MaxHp;
            _hpBar.Init(_unitModel.MaxHp);
        }

        public int GetCollisionDamage() => _unitModel.CollisionDamage;

        public virtual void TakeDamage(int damage)
        {
            _currentHp = Mathf.Max(0, _currentHp - damage);
            _hpBar.UpdateState(_currentHp);
            if (_currentHp <= 0)
            {
                OnDied?.Invoke();
                Died();
            }
        }

        protected void TakeLethalDamage()
        {
            TakeDamage(_currentHp);
        }

        public abstract void Died();
    
        public event Action OnDied;
    }
}