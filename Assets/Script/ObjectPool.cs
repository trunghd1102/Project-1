using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Stack<T> pooledObjects = new Stack<T>();
    private readonly T prefab;
    private readonly int initialSize;
    private readonly Transform poolParent; // Parent để tổ chức các object trong Hierarchy

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        poolParent = new GameObject($"Pool_{typeof(T).Name}").transform;
        poolParent.SetParent(null); // Đặt parent ở root

        // Khởi tạo pool
        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            pooledObjects.Push(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolParent);
        return obj;
    }

    public T Get()
    {
        T obj;
        if (pooledObjects.Count > 0)
        {
            obj = pooledObjects.Pop();
        }
        else
        {
            obj = CreateNewObject();
        }
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pooledObjects.Push(obj);
    }
}