using PaperBoy.Sets;
using UnityEngine;

namespace PaperBoy.Procedural
{
    [CreateAssetMenu(fileName = "StructureSO_", menuName = "ScriptableObjects/PaperBoy/Procedural/StructureSO", order = 1)]
    public class StructureSO : ScriptableObject
    {
        [Range(1, 30)]public float Length;
        [Range(0, 1)] public float BorderSpawnChance;
        public ObjectPoolSet BorderPoolSet;
    }
}