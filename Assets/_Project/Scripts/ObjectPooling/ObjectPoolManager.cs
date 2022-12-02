using System;
using System.Collections.Generic;
using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.ObjectPool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private List<ObjectPoolSet> objectPoolSets;


        private void OnValidate()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                ObjectPool objectPool = child.GetComponent<ObjectPool>();
                ObjectPoolSet objectPoolSet = objectPool.GetObjectPoolSO().ObjectPoolSet;
                if (objectPoolSets.Contains(objectPoolSet))
                {
                    continue;
                }
                
                objectPoolSets.Add(objectPoolSet);
            }
        }

        private void Start()
        {
            foreach (ObjectPoolSet objectPoolSet in objectPoolSets)
            {
                foreach (var objectPool in objectPoolSet.Items)
                {
                    objectPool.Initialize();
                }
            }
        }
    }
}
