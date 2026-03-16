using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected int initialSize = 10;
    [SerializeField] protected GameObject _parentObject;

    protected Queue<IPooledObject> Pool = new Queue<IPooledObject>();
    protected List<IPooledObject> AllObjects = new List<IPooledObject>();
    
    protected IPooledObject _spawnPrefab;
    public void SetPrefab(GameObject prefab) => _spawnPrefab = prefab.GetComponent<IPooledObject>();

    protected void CreateNewObject()
    {
        var obj = Instantiate(_spawnPrefab.Monobehaviour, _parentObject.transform, true);
        obj.SetActive(false);
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        Pool.Enqueue(pooledObj);
        AllObjects.Add(pooledObj);
    }

    protected GameObject Get()
    {
        if (Pool.Count == 0)
            CreateNewObject();

        var obj = Pool.Dequeue();
        return obj.Monobehaviour;
    }

    public void SetParent(GameObject obj)
    {
        obj.transform.SetParent(_parentObject.transform);
    }

    protected void ReturnAll()
    {
        foreach (var obj in AllObjects)
        {
            ReturnToPool(obj);
        }
    }

    public void ReturnToPool(IPooledObject obj)
    {
        obj.Monobehaviour.SetActive(false);
        Pool.Enqueue(obj);
    }
}
