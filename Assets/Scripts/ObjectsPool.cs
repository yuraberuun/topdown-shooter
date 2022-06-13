using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using System.Threading.Tasks;

using UnityEngine;

public class ObjectsPool
{
    List<GameObject> _objectsPool;

    GameObject _prefab;

    Transform _parentNode;

    public ObjectsPool(int startSize, GameObject prefab, GameObject parentNode)
    {
        _prefab = prefab;
        _parentNode = parentNode.transform;

        _objectsPool = new List<GameObject>();

        for (int i = 0; i < startSize; ++i)
            AddObject(InstantiateNewObject());
    }

    GameObject InstantiateNewObject()
    {
        GameObject gameObject = GameObject.Instantiate(_prefab, _parentNode);

        return gameObject;
    }

    public void AddObject(GameObject gameObject) //Цей метод повинен викликатися в момент, коли об'єкт стає непотрібним
    {
        gameObject.SetActive(false);

        _objectsPool.Add(gameObject);
    }

    public GameObject GetObject()
    {
        if(_objectsPool.Count == 0)
            return InstantiateNewObject();

        GameObject gameObject;

        gameObject = _objectsPool[0];
        gameObject.SetActive(true);

        _objectsPool.RemoveAt(0);

        return gameObject;
    }
}