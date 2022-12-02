using System;
using System.Collections.Generic;
using Core.Debug;
using PaperBoy.ObjectPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PaperBoy.Procedural
{
    public class RandomObjectSpawner : MonoBehaviour
    {
        [SerializeField] private RandomObjectSpawnerSO randomObjectSpawnerSO;
        [SerializeField] private List<Transform> spawnPoints;

        private int _total;
        private List<PoolableObject> _spawnedObjects = new List<PoolableObject>();
        

        public void CalculateTotalWeight()
        {
            if (_total > 0)
            {
                return;
            }

            foreach (WeightedObjectSO weightedObjectSO in randomObjectSpawnerSO.WeightedObjectSOs)
            {
                int weight = weightedObjectSO.Weight;
                if (weight <= 0)
                {
                    continue;
                }

                _total += weight;
            }
        }

        public void SpawnObjects()
        {
            if (_total <= 0)
            {
                CustomLogger.Warning(nameof(SpawnObjects), $"{nameof(_total)}={_total}", this);
                return;
            }
            
            foreach (Transform spawnPoint in spawnPoints)
            {
                Roll(spawnPoint);
            }
        }

        private void Roll(Transform spawnPoint)
        {
            float random = Random.Range(0, _total);
            float roll = 0;
            for (int i = 0; i < randomObjectSpawnerSO.WeightedObjectSOs.Count; i++)
            {
                float weight = randomObjectSpawnerSO.WeightedObjectSOs[i].Weight;
                if (weight <= 0)
                {
                    continue;
                }

                roll += weight / _total;
                if (roll >= random)
                {
                    Spawn(i, spawnPoint);
                    break;
                }
            }
        }

        private void Spawn(int index, Transform spawnPoint)
        {
            ObjectPoolSO objectPoolSO = randomObjectSpawnerSO.WeightedObjectSOs[index].ObjectPoolSO;
            if (!objectPoolSO)
            {
                return;
            }

            foreach (ObjectPool.ObjectPool objectPool in objectPoolSO.ObjectPoolSet.Items)
            {
                if (objectPool.GetObjectPoolSO() == objectPoolSO)
                {
                    PoolableObject pooledObject = objectPool.GetPooledObject();
                    if (!pooledObject)        //Don't spawn Object
                    {
                        return;
                    }

                    pooledObject.Spawn(spawnPoint.position, spawnPoint.rotation, spawnPoint);
                    _spawnedObjects.Add(pooledObject);
                    return;
                }
            }
        }

        public void DespawnObjects()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                PoolableObject spawnedObject = _spawnedObjects[i];
                _spawnedObjects.RemoveAt(i);
                spawnedObject.Despawn();
            }
        }
    }
}
