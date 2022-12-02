using System;
using System.Collections.Generic;
using Core.Debug;
using Core.UpdateManager;
using PaperBoy.ObjectPool;
using PaperBoy.Procedural;
using PaperBoy.Sets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PaperBoy.LevelScrolling
{
    public class Treadmill : UpdateableBehaviour
    {
        [SerializeField] private TreadmillSO treadmillSO;
        [SerializeField] private ObjectPoolSet objectPoolSet;
        [SerializeField] private Transform spawnOrigin;

        private float _spawnOriginX;
        private float _spawnOriginY;
        private float _spawnOriginZ;
        private Vector3 _movement;
        private Quaternion _spawnRotation;
        private ObjectPool.ObjectPool _objectPool;
        private List<Tile> _spawnedTiles = new List<Tile>();
        

        protected override void Awake()
        {
            base.Awake();
            
            Vector3 spawnOriginPosition = spawnOrigin.position;
            _spawnOriginX = spawnOriginPosition.x;
            _spawnOriginY = spawnOriginPosition.y;
            _spawnOriginZ = spawnOriginPosition.z;
            _spawnRotation = spawnOrigin.rotation;
        }

        private void Start()
        {
            SpawnTile(_spawnOriginZ);

            Tile lastTile = _spawnedTiles[_spawnedTiles.Count - 1];
            float farthestPointZ = lastTile.GetFarthestPointZ();
            while (farthestPointZ < treadmillSO.TreadmillLength)
            {
                SpawnTile(farthestPointZ);
                lastTile = _spawnedTiles[_spawnedTiles.Count - 1];
                farthestPointZ = lastTile.GetFarthestPointZ();
            }
        }

        public override void ManagedLateUpdate()
        {
            _movement = Vector3.back * (treadmillSO.MovementSpeed * Time.deltaTime);        //Moves back in worldspace
            
            for (int i = _spawnedTiles.Count - 1; i >= 0; i--)
            {
                Tile tile = _spawnedTiles[i];
                tile.Move(_movement);
                if (tile.GetFarthestPointZ() < _spawnOriginZ)
                {
                    _spawnedTiles.Remove(tile);
                    tile.Despawn();
                }
            }

            Tile lastTile = _spawnedTiles[_spawnedTiles.Count - 1];
            float farthestPointZ = lastTile.GetFarthestPointZ();
            if (farthestPointZ < treadmillSO.TreadmillLength)
            {
                SpawnTile(farthestPointZ);
            }
        }

        private void SpawnTile(float farthestPointZ)
        {
            int randomIndex = Random.Range(0, objectPoolSet.Items.Count);
            _objectPool = objectPoolSet.Items[randomIndex];
            
            PoolableObject pooledObject = _objectPool.GetPooledObject();
            Tile spawnedTile = pooledObject.GetComponent<Tile>();
            Vector3 spawnPosition = spawnedTile.GetSpawnPosition(_spawnOriginX, _spawnOriginY, farthestPointZ);
            pooledObject.Spawn(spawnPosition, _spawnRotation);
            _spawnedTiles.Add(spawnedTile);
        }

        public float GetTreadmillSpeed()
        {
            return treadmillSO.MovementSpeed;
        }
    }
}

