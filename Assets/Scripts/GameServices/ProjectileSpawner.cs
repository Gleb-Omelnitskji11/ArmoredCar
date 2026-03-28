using System.Collections.Generic;
using ConfigData;
using Core;
using Core.BusEvents;
using Core.ObjectPool;
using GameUnits;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class ProjectileSpawner
    {
        private GameConfig _gameConfig;
        private IObjectPooler _pooler;
        
        private ProjectaleModel _projectileModel;
        private ProjectileType _projectileType;
        private IEventBus _eventBus;
        
        private List<BasicProjectile> _projectiles = new List<BasicProjectile>();

        [Inject]
        public void Construct(IObjectPooler pooler, ConfigProvider configProvider, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _pooler = pooler;
            _gameConfig = configProvider.GameConfig;
        }
        
        public void Subscribe()
        {
            _eventBus.Subscribe<GameResultEvent>(ClearPool);
            _eventBus.Subscribe<PauseEvent>(OnPause);
        }
        
        ~ProjectileSpawner()
        {
            _eventBus.Unsubscribe<GameResultEvent>(ClearPool);
            _eventBus.Unsubscribe<PauseEvent>(OnPause);
        }

        public void SetDataForLevel(ProjectileType projectileType)
        {
            _projectileType = projectileType;
            InitPool();
        }

        public T SpawnBullet<T>() where T : BasicProjectile
        {
            string key = GetKey(_projectileType);
            BasicProjectile enemy = _pooler.Get<BasicProjectile, ProjectaleModel>(key);
            return enemy as T;
        }

        private void OnPause(PauseEvent pauseEvent)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Pause(pauseEvent.IsPause);
            }
        }

        private void ClearPool(GameResultEvent restartEvent)
        {
            var temp = new List<BasicProjectile>(_projectiles);
            foreach (var projectile in temp)
            {
                projectile.Deactivate();
            }
            string key = GetKey(_projectileType);
            _pooler.Clear(key);
            _projectiles.Clear();
        }
        
        private void InitPool()
        {
            ProjectaleModel model = _gameConfig.GetProjectileModel(_projectileType);
            string key = GetKey(_projectileType);

            _pooler.CreatePool<BasicProjectile, ProjectaleModel>(key, model, factory: CreateNewProjectileObj,
                onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                prewarmCount: 0);
        }
        
        private BasicProjectile CreateNewProjectileObj(ProjectaleModel model)
        {
            BasicProjectile projectile = GameObject.Instantiate(model.BulletPrefab);
            ProjectileType projectileType = model.ProjectileType;
            string key = GetKey(projectileType);
            projectile.SetProjectileModel(model);
            projectile.SetPoolData(_pooler, key);
            return projectile;
        }
        
        private void OnGetFromPool(BasicProjectile projectile)
        {
            _projectiles.Add(projectile);
        }
        
        private void OnRealiseToPool(BasicProjectile projectile)
        {
            _projectiles.Remove(projectile);
        }
        
        private string GetKey(ProjectileType type) => $"Projectile_{type}";
    }
}