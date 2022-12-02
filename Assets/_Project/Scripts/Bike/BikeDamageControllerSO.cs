using UnityEngine;

namespace PaperBoy.Bike
{
    [CreateAssetMenu(fileName = "BikeDamageControllerSO_", menuName = "ScriptableObjects/PaperBoy/Bike/BikeDamageControllerSO", order = 0)]
    public class BikeDamageControllerSO : ScriptableObject
    {
        public float Cooldown;
        public int HitDamage;
    }
}
