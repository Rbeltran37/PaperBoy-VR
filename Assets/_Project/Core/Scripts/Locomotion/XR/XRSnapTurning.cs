using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Locomotion.XR
{
    public class XRSnapTurning : XRTurningSolution
    {
        private bool _isTurning;
        private float _offSensitivity = .2f;        //Cached for performance
        
        
        public override List<float> GetTurnSpeeds()
        {
            return xrLocomotionSo.SnapTurnSpeeds;
        }
        
        public override void AttemptTurn(Vector2 axisValue)
        {
            float x = Math.Abs(axisValue.x);
            if (x < _offSensitivity)
            {
                _isTurning = false;
                return;
            }
            
            if (x < OnSensitivity)
            {
                return;
            }
            
            if (_isTurning)
            {
                return;
            }
            
            Turn(axisValue, CurrentTurnSpeed);
        }

        protected override void Turn(Vector2 axisValue, float turnSpeed)
        {
            _isTurning = true;

            int angleSign = 1;
            if (axisValue.x < 0)
            {
                angleSign = -1;
            }

            PlayArea.RotateAround(Headset.position, Vector3.up, angleSign * turnSpeed);
        }

        protected override void SetOnSensitivity()
        {
            OnSensitivity = xrLocomotionSo.SnapOnSensitivity;
            _offSensitivity = OnSensitivity - xrLocomotionSo.SnapOffBuffer;
        }
    }
}
