using System;
using UnityEngine;

namespace PaperBoy.Hands
{
    [Serializable]
    public class FingerJoint
    {
        public Vector3 Position;
        public Quaternion Rotation;


        public void Set(Transform transform)
        {
            Position = transform.localPosition;
            Rotation = transform.localRotation;
        }
    }
}
