using System;
using System.Collections.Generic;
using PaperBoy.LevelScrolling;
using PaperBoy.ObjectPool;
using UnityEngine;

namespace PaperBoy.Procedural
{
    public class Tile : PoolableObject, ITileable
    {
        [SerializeField] private TileSO tileSO;
        [SerializeField] private Transform structureSpawnPoint;
        [SerializeField] private Transform mailboxSpawnPoint;

        private PoolableObject _spawnedStructure;
        private PoolableObject _spawnedMailbox;
        private RandomObjectSpawner[] _randomObjectSpawners;


        protected override void Awake()
        {
            base.Awake();

            _randomObjectSpawners = GetComponentsInChildren<RandomObjectSpawner>();
        }

        private void OnEnable()
        {
            SpawnStructure();
            SpawnMailbox();
            
            SpawnRandomObjects();
        }

        private void SpawnStructure()
        {
            ObjectPool.ObjectPool objectPool = tileSO.GetRandomStructurePool();
            if (!objectPool)
            {
                return;
            }
            
            Vector3 position = structureSpawnPoint.position;
            Quaternion rotation = structureSpawnPoint.rotation;

            _spawnedStructure = objectPool.GetPooledObject();
            _spawnedStructure.Spawn(position, rotation, structureSpawnPoint);
        }

        private void SpawnMailbox()
        {
            ObjectPool.ObjectPool objectPool = tileSO.GetRandomMailboxPool();
            if (!objectPool)
            {
                return;
            }
            
            Vector3 position = mailboxSpawnPoint.position;
            Quaternion rotation = mailboxSpawnPoint.rotation;
            _spawnedMailbox = objectPool.GetPooledObject();
            _spawnedMailbox.Spawn(position, rotation, mailboxSpawnPoint);
        }
        
        public override void Despawn()
        {
            if (_spawnedStructure)
            {
                _spawnedStructure.Despawn();
                _spawnedStructure = null;
            }

            if (_spawnedMailbox)
            {
                _spawnedMailbox.Despawn();
                _spawnedMailbox = null;
            }
            
            DespawnRandomObjects();

            base.Despawn();
        }

        public void Move(Vector3 movement)
        {
            ThisTransform.Translate(movement, Space.World);
        }

        public float GetFarthestPointZ()
        {
            return ThisTransform.position.z + tileSO.Length / 2;
        }

        public Vector3 GetSpawnPosition(float worldX, float worldY, float worldZ)
        {
            return new Vector3(worldX, worldY, worldZ + tileSO.Length / 2);
        }
        
        private void SpawnRandomObjects()
        {
            foreach (RandomObjectSpawner randomObjectSpawner in _randomObjectSpawners)
            {
                randomObjectSpawner.CalculateTotalWeight();
                randomObjectSpawner.SpawnObjects();
            }
        }

        private void DespawnRandomObjects()
        {
            foreach (RandomObjectSpawner randomObjectSpawner in _randomObjectSpawners)
            {
                randomObjectSpawner.DespawnObjects();
            }
        }
    }
}
