using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.ObjectPool
{
    [CreateAssetMenu(fileName = "ObjectPoolSO_", menuName = "ScriptableObjects/PaperBoy/ObjectPool/ObjectPoolSO", order = 1)]
    public class ObjectPoolSO : ScriptableObject
    {
        public ObjectPoolSet ObjectPoolSet;
        public GameObject PrefabToPool;
        public int NumObjectsToPool;
    }
}

