using System;
using System.Collections.Generic;
using Core.Locomotion.XR;
using UnityEngine;

namespace Core
{
    public class XRSmoothLocomotion : XRMovementSolution
    {
        public override void AttemptLocomotion(Vector2 axisValue)
        {
            float x = Math.Abs(axisValue.x);
            float y = Math.Abs(axisValue.y);
            if (x < OnSensitivity &&
                y < OnSensitivity)
            {
                return;
            }
            
            Locomotion(axisValue, CurrentMovementSpeed);
        }

        public override List<float> GetMovementSpeeds()
        {
            return xrLocomotionSo.SmoothLocomotionSpeeds;
        }

        protected override void Locomotion(Vector2 axisValue, float movementSpeed)
        {
            Vector3 direction = Quaternion.Euler(new Vector3(0, Headset.eulerAngles.y, 0)) * new Vector3(axisValue.x, 0, axisValue.y);
            PlayArea.transform.position += direction * movementSpeed;
        }

        protected override void SetOnSensitivity()
        {
            OnSensitivity = xrLocomotionSo.SmoothLocomotionOnSensitivity;
        }
    }
}
