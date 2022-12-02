using System.Collections.Generic;
using UnityEngine;

namespace PaperBoy.Procedural
{
    [CreateAssetMenu(fileName = "RandomObjectSpawnerSO_", menuName = "ScriptableObjects/PaperBoy/Procedural/RandomObjectSpawnerSO", order = 1)]
    public class RandomObjectSpawnerSO : ScriptableObject
    {
        public List<WeightedObjectSO> WeightedObjectSOs;
    }
}
