using Core.Locomotion.XR;
using UnityEngine;

namespace PaperBoy.Bike
{
    [CreateAssetMenu(fileName = "BikeLocomotionAnimatorSO_", menuName = "ScriptableObjects/PaperBoy/Bike/BikeLocomotionAnimatorSO", order = 1)]
    public class BikeLocomotionAnimatorSO : ScriptableObject
    {
        public XRLocomotionSO XrLocomotionSO;
        [Range(0f, 1f)] public float HandleBarTurnMultiplier = .5f;
        [Range(0.1f, 3f)] public float HandleBarTurnLerpTime = .25f;        //Higher value = slower
        [Range(0f, 1f)] public float BikeTurnMultiplier = .3f;
        [Range(0.1f, 3f)] public float BikeTurnLerpTime = 1f;        //Higher value = slower
        [Range(0f, 1f)] public float BikeLeanMultiplier = .3f;
        [Range(0.1f, 3f)] public float BikeLeanLerpTime = 1f;        //Higher value = slower
    }
}
