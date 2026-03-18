using System;
using System.Collections;
using ConfigData;
using Core;
using Core.BusEvents;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace GameUnits
{
    public class PlayerCar : Unit
    {
        [SerializeField] private Turret _turret;
        [SerializeField] private Transform[] _wheels;
        [SerializeField] private TrailRenderer[] _trails;

        private IEnumerator _moveCoroutine;

        private Vector3 _carPosition;
        private Sequence _seq;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void InitUnit(UnitModel model, TurretModel turretModel)
        {
            base.InitUnit(model);
            _turret.Init(turretModel);
            _hpBar.Init(model.MaxHp);
        }

        public void StartLevel()
        {
            foreach (var trail in _trails)
            {
                trail.Clear();
            }
            
            _seq?.Kill();
            UpdateDirection();
            _turret.Activate();
        }

        private void UpdateDirection()
        {
            float newZ = transform.position.z + 9999f;
            float duration = 9999f / _unitModel.Speed;
            _seq = DOTween.Sequence();
            _seq.Join(transform.DOMoveZ(newZ, duration))
                .SetEase(Ease.Linear);
            foreach (var wheel in _wheels)
            {
                _seq.Join(wheel.DORotate(new Vector3(360f, 0f, 0f), 2f, RotateMode.FastBeyond360));
            }
            _seq.OnComplete(UpdateDirection);
        }

        public override void TakeDamage(int damageTaken)
        {
            base.TakeDamage(damageTaken);
        }

        public override void Died()
        {
            GameResultEvent gameResultEvent = new GameResultEvent(false);
            _eventBus.Publish<GameResultEvent>(gameResultEvent);
            Stop();
        }

        public void Stop()
        {
            _seq?.Kill();
            _turret.Stop();
        }
    }
}