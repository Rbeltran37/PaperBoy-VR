using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.Procedural
{
    [CreateAssetMenu(fileName = "TileSO_", menuName = "ScriptableObjects/PaperBoy/Procedural/TileSO", order = 1)]
    public class TileSO : ScriptableObject
    {
        public float Length;
        
        [Header("Structure")]
        public ObjectPoolSet StructurePoolSet;
        public ObjectPoolSet MailboxSet;

        public ObjectPool.ObjectPool GetRandomStructurePool()
        {
            return GetRandomObjectPool(StructurePoolSet);
        }
        
        public ObjectPool.ObjectPool GetRandomMailboxPool()
        {
            return GetRandomObjectPool(MailboxSet);
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
    }
}
