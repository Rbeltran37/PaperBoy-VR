using System;
using System.Collections.Generic;
using Core.Debug;
using UnityEngine;

namespace PaperBoy.ObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] protected ObjectPoolSO ObjectPoolSO;
        
        private bool _hasInitialized;
        private int _numPooled;
        private Transform _thisTransform;
        private List<PoolableObject> _pooledObjects = new List<PoolableObject>();
        

        private string Name => $"Pooled {ObjectPoolSO.PrefabToPool.name} ({_numPooled++})";


        private void Awake()
        {
            ObjectPoolSO.ObjectPoolSet.Add(this);
        }

        private void OnEnable()
        {
            ObjectPoolSO.ObjectPoolSet.Add(this);
        }

        private void OnDisable()
        {
            ObjectPoolSO.ObjectPoolSet.Remove(this);
        }

        public void Initialize()
        {
            if (_hasInitialized)
            {
                return;
            }

            _hasInitialized = true;
            _thisTransform = transform;

            FillPool();
        }

        private void FillPool()
        {
            while (_pooledObjects.Count < ObjectPoolSO.NumObjectsToPool)
            {
                InstantiatePoolableObject();
            }
        }

        private void InstantiatePoolableObject()
        {
            GameObject currentGameObject = Instantiate(ObjectPoolSO.PrefabToPool, _thisTransform);
            currentGameObject.name = Name;
            PoolableObject currentPoolable = currentGameObject.GetComponent<PoolableObject>();
            currentPoolable.SetObjectPool(this);
            _pooledObjects.Add(currentPoolable);
        }
        
        public PoolableObject GetPooledObject()
        {
            if (_pooledObjects.Count == 0)
            {
                InstantiatePoolableObject();
                return GetPooledObject();
            }
            
            PoolableObject pooledObject = _pooledObjects[0];
            _pooledObjects.Remove(pooledObject);
            
            return pooledObject;
        }

        public void Pool(PoolableObject poolableObject)
        {
            _pooledObjects.Add(poolableObject);
        }
        
        public ObjectPoolSO GetObjectPoolSO()
        {
            return ObjectPoolSO;
        }
    }
}
