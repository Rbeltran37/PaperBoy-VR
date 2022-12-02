using System.Collections.Generic;
using UnityEngine;

namespace Core.Locomotion.XR
{
    [CreateAssetMenu(fileName = "XRLocomotionSO", menuName = "ScriptableObjects/Core/Locomotion/XRLocomotionSO", order = 1)]
    public class XRLocomotionSO : ScriptableObject
    {
        [Header("Smooth Turning")] 
        [Range(.1f, 1f)] public float SmoothTurnOnSensitivity = .1f;
        public List<float> SmoothTurnSpeeds;
        
        [Header("Snap Turning")]
        [Range(.1f, 1f)] public float SnapOnSensitivity = .2f;
        [Range(.05f, .25f)] public float SnapOffBuffer = .05f;
        public List<float> SnapTurnSpeeds;
        
        [Header("Smooth Locomotion")]
        [Range(.1f, 1f)] public float SmoothLocomotionOnSensitivity = .1f;
        public List<float> SmoothLocomotionSpeeds;
        
        [Header("Lateral Locomotion")]
        [Range(.1f, 1f)] public float LateralLocomotionOnSensitivity = .1f;
        public List<float> LateralLocomotionSpeeds;
    }
}
