using System;
using Core.Events;
using PaperBoy.ObjectPool;
using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.Bike
{
    public class BikeBasket : MonoBehaviour
    {
        [SerializeField] private ObjectPoolSet objectPoolSet;
        [SerializeField] private RuntimeGameEventListener fillBasketEventListener;
        [SerializeField] private Transform[] spawnTransforms;

        private ObjectPool.ObjectPool _objectPool;

        private const int MAX_PAPERS_IN_BASKET = 9;


        private void OnEnable()
        {
            fillBasketEventListener.Subscribe(FillBasket);
        }

        private void OnDisable()
        {
            fillBasketEventListener.Unsubscribe(FillBasket);
        }

        private void Start()
        {
            _objectPool = objectPoolSet.Items[0];
            
            FillBasket();
        }

        private void FillBasket()
        {
            for (int i = 0; i < MAX_PAPERS_IN_BASKET; i++)
            {
                Transform currentTransform = spawnTransforms[i];
                if (currentTransform.childCount > 0)
                {
                    continue;
                }
                
                PoolableObject pooledObject = _objectPool.GetPooledObject();
                Transform spawnTransform = spawnTransforms[i];
                pooledObject.Spawn(spawnTransform.position, spawnTransform.rotation, spawnTransform);
            }
        }
    }
}
