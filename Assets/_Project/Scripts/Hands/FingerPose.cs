using System;
using Core.Debug;
using UnityEngine;

namespace PaperBoy.Hands
{
    [Serializable]
    public class FingerPose
    {
        public Finger Finger;
        public FingerJoint[] Joints = new FingerJoint[NUM_JOINTS];

        public const int NUM_JOINTS = 3;
        

        public void Set(Transform transform)
        {
            Transform joint = transform;
            for (int i = 0; i < NUM_JOINTS; i++)
            {
                CustomLogger.Info(nameof(Set), $"{joint.name} Joint copied", joint);
                Joints[i].Set(joint);
                if (joint.childCount == 0)
                {
                    break;
                }
                
                joint = joint.GetChild(0);
            }
        }
    }
}