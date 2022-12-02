using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Locomotion.XR
{
    public class XRSmoothTurning : XRTurningSolution
    {
        public override List<float> GetTurnSpeeds()
        {
            return xrLocomotionSo.SmoothTurnSpeeds;
        }
        
        public override void AttemptTurn(Vector2 axisValue)
        {
            float x = Math.Abs(axisValue.x);
            if (x < OnSensitivity)
            {
                return;
            }

            Turn(axisValue, CurrentTurnSpeed);
        }

        protected override void Turn(Vector2 axisValue, float turnSpeed) 
        {
            PlayArea.RotateAround(Headset.position, Vector3.up, axisValue.x * turnSpeed);
        }

        protected override void SetOnSensitivity()
        {
            OnSensitivity = xrLocomotionSo.SmoothTurnOnSensitivity;
        }
    }
}
