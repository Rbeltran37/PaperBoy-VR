using PaperBoy.ObjectPool;
using PaperBoy.Sets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PaperBoy.Procedural
{
    public class Structure : PoolableObject
    {
        [SerializeField] private StructureSO structureSO;

        private PoolableObject spawnedBorders;
        
        public StructureSO StructureSO => structureSO;


        private void OnEnable()
        {
            SpawnBorder();
        }

        public override void Despawn()
        {
            if (spawnedBorders)
            {
                spawnedBorders.Despawn();
            }
            
            base.Despawn();
        }

        private void SpawnBorder()
        {
            ObjectPool.ObjectPool objectPool = GetRandomBorderPool();
            if (!objectPool)
            {
                return;
            }
            
            Vector3 position = ThisTransform.position;
            Quaternion rotation = ThisTransform.rotation;

            spawnedBorders = objectPool.GetPooledObject();
            spawnedBorders.Spawn(position, rotation, ThisTransform);
        }
        
        public ObjectPool.ObjectPool GetRandomBorderPool()
        {
            if (!ShouldSpawn(structureSO.BorderSpawnChance))
            {
                return null;
            }
            
            return GetRandomObjectPool(structureSO.BorderPoolSet);
        }
        
        private ObjectPool.ObjectPool GetRandomObjectPool(ObjectPoolSet objectPoolSet)
        {
            if (!objectPoolSet || objectPoolSet.Items.Count == 0)
            {
                return null;
            }
            
            int index = Random.Range(0, objectPoolSet.Items.Count);
            ObjectPool.ObjectPool objectPool = objectPoolSet.Items[index];

            return objectPool;
        }

        private bool ShouldSpawn(float spawnChance)
        {
            float chance = Random.Range(0f, 1f);
            return chance <= spawnChance;
        }
    }
}
