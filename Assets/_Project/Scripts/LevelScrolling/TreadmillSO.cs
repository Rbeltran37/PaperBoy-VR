using UnityEngine;

namespace PaperBoy.LevelScrolling
{
    [CreateAssetMenu(fileName = "TreadmillSO", menuName = "ScriptableObjects/PaperBoy/LevelScrolling/TreadmillSO", order = 1)]
    public class TreadmillSO : ScriptableObject
    {
        public float MovementSpeed;
        public float TreadmillLength;
    }
}
