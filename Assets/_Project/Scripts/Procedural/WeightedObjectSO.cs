using PaperBoy.ObjectPool;
using UnityEngine;

namespace PaperBoy.Procedural
{
    [CreateAssetMenu(fileName = "WeightedObjectSO_", menuName = "ScriptableObjects/PaperBoy/Procedural/WeightedObjectSO", order = 1)]
    public class WeightedObjectSO : ScriptableObject
    {
        public int Weight;
        public ObjectPoolSO ObjectPoolSO;
    }
}
