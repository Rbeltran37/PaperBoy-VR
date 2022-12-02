using System.Collections.Generic;
using Core.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Locomotion.XR
{
    public abstract class XRMovementSolution : MonoBehaviour
    {
        [SerializeField] protected XRLocomotionSO xrLocomotionSo;
        [SerializeField] protected Transform PlayArea;
        [SerializeField] protected Transform Headset;

        protected float OnSensitivity = .25f;       //Cached for performance
        protected float CurrentMovementSpeed = 3;

        
        public abstract void AttemptLocomotion(Vector2 axisValue);
        public abstract List<float> GetMovementSpeeds();
        protected abstract void Locomotion(Vector2 axisValue, float movementSpeed);

        protected abstract void SetOnSensitivity();

        public void Initialize(Transform playArea, Transform headset)
        {
            SetPlayArea(playArea);
            SetHeadset(headset);
            SetOnSensitivity();
        }

        private void SetPlayArea(Transform playArea)
        {
            PlayArea = playArea;
        }
        
        private void SetHeadset(Transform headset)
        {
            Headset = headset;
        }

        public void SetCurrentMovementSpeed(float speed)
        {
            CurrentMovementSpeed = speed;
        }
        
        public void ReadInput(InputAction.CallbackContext callbackContext)
        {
            Vector2 axisValue = callbackContext.ReadValue<Vector2>();
            AttemptLocomotion(axisValue);
        }
    }
}
