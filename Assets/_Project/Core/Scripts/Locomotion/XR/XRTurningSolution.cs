using System.Collections.Generic;
using Core.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Locomotion.XR
{
    public abstract class XRTurningSolution : MonoBehaviour
    {
        [SerializeField] protected XRLocomotionSO xrLocomotionSo;
        [SerializeField] protected Transform PlayArea;
        [SerializeField] protected Transform Headset;
        
        protected float OnSensitivity = .25f;       //Cached for performance
        protected float CurrentTurnSpeed = 30;


        public abstract void AttemptTurn(Vector2 axisValue);
        public abstract List<float> GetTurnSpeeds();

        protected abstract void Turn(Vector2 axisValue, float turnSpeed);
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

        public void SetCurrentTurnSpeed(float speed)
        {
            CurrentTurnSpeed = speed;
        }
        
        public void ReadInput(InputAction.CallbackContext callbackContext)
        {
            Vector2 axisValue = callbackContext.ReadValue<Vector2>();
            AttemptTurn(axisValue);
        }
    }
}
