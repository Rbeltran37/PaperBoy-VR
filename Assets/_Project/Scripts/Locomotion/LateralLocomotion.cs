using System;
using System.Collections.Generic;
using Core.Locomotion.XR;
using UnityEngine;

namespace PaperBoy.Locomotion
{
    public class LateralLocomotion : XRMovementSolution
    {
        public override void AttemptLocomotion(Vector2 axisValue)
        {
            float x = Math.Abs(axisValue.x);
            if (x < OnSensitivity)
            {
                return;
            }
            
            Locomotion(axisValue, CurrentMovementSpeed);
        }

        public override List<float> GetMovementSpeeds()
        {
            return xrLocomotionSo.LateralLocomotionSpeeds;
        }

        protected override void Locomotion(Vector2 axisValue, float movementSpeed)
        {
            Vector3 direction = new Vector3(axisValue.x, 0, 0);
            PlayArea.transform.localPosition += direction * movementSpeed;
        }

        protected override void SetOnSensitivity()
        {
            OnSensitivity = xrLocomotionSo.LateralLocomotionOnSensitivity;
        }
    }
}
