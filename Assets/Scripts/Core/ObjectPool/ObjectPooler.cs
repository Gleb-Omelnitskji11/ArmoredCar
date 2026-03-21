using System;
using System.Collections.Generic;
using GameUnits;
using UnityEngine;

namespace Core.ObjectPool
{
    public class ObjectPooler : IObjectPooler
    {
        private readonly Dictionary<string, IPool> _pools = new Dictionary<string, IPool>();

        public void Init()
        {
            ClearAll();
        }

        public void CreatePool<T>(string key, T prefab, Func<T, T> factory, Action<T> onGet = null, Action<T> onRelease = null, int prewarmCount = 0) where T : IPooledObject
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Pool key cannot be null or empty.", nameof(key));
            }

            var pool = new Pool<T>(prefab, factory, onGet, onRelease, prewarmCount);
            if (!_pools.TryAdd(key, pool))
            {
                Debug.Log($"Pool with key '{key}' already exists. Skipping creation.");
            }
        }

        public T Get<T>(string key) where T : IPooledObject
        {
            if (!_pools.TryGetValue(key, out var poolBase))
            {
                throw new InvalidOperationException($"Pool with key '{key}' does not exist.");
            }

            if (poolBase is Pool<T> pool)
            {
                return pool.Get();
            }

            throw new InvalidOperationException($"Pool with key '{key}' is not of type {typeof(T).Name}.");
        }

        public void Release<T>(string key, T instance) where T : IPooledObject
        {
            if (!_pools.TryGetValue(key, out var poolBase))
            {
                Debug.LogWarning($"Pool with key '{key}' does not exist. Cannot release instance.");
                return;
            }

            if (poolBase is Pool<T> pool)
            {
                pool.Release(instance);
            }
            else
            {
                Debug.LogWarning($"Pool with key '{key}' is not of type {typeof(T).Name}. Cannot release instance.");
            }
        }

        public void Clear(string key)
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                pool.Clear();
            }
        }

        public void ClearAll()
        {
            foreach (var pool in _pools.Values)
            {
                pool.Clear();
            }
            _pools.Clear();
        }
        
        
        private interface IPool
        {
            void Clear();
        }

        private class Pool<T> : IPool where T : IPooledObject
        {
            private readonly T _prefab;
            private readonly Stack<T> _available;
            private readonly Func<T, T> _factory;
            private readonly Action<T> _onGet;
            private readonly Action<T> _onRelease;

            public Pool(T prefab, Func<T, T> factory, Action<T> onGet, Action<T> onRelease, int prewarmCount)
            {
                _prefab = prefab;
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
                _onGet = onGet;
                _onRelease = onRelease;
                _available = new Stack<T>(prewarmCount);

                for (int i = 0; i < prewarmCount; i++)
                {
                    var instance = _factory(_prefab);
                    _available.Push(instance);
                }
            }

            public T Get()
            {
                T instance;
                if (_available.Count > 0)
                {
                    instance = _available.Pop();
                }
                else
                {
                    instance = _factory(_prefab);
                }

                _onGet?.Invoke(instance);
                return instance;
            }

            public void Release(T instance)
            {
                if (instance == null)
                {
                    return;
                }

                _onRelease?.Invoke(instance);
                _available.Push(instance);
            }

            public void Clear()
            {
                _available.Clear();
            }
        }
    }
}

