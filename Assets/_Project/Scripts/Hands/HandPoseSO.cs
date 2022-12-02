using System;
using UnityEngine;

namespace PaperBoy.Hands
{
    [CreateAssetMenu(fileName = "HandPoseSO_", menuName = "ScriptableObjects/PaperBoy/Hands/HandPoseSO", order = 1)]
    public class HandPoseSO : ScriptableObject
    {
        public HandPose HandPose;

        private readonly Finger[] _fingers = Enum.GetValues(typeof(Finger)) as Finger[];

        public const int NUM_FINGERS = 5;
        
        private const int THUMB = 0;
        private const int INDEX = 1;
        private const int MIDDLE = 2;
        private const int RING = 3;
        private const int PINKY = 4;
        

        private void OnValidate()
        {
            if (HandPose == null)
            {
                return;
            }

            int fingerCount = HandPose.FingerPoses.Count;
            if (fingerCount != NUM_FINGERS)
            {
                while (HandPose.FingerPoses.Count < NUM_FINGERS)
                {
                    HandPose.FingerPoses.Add(new FingerPose());
                }

                if (HandPose.FingerPoses.Count > NUM_FINGERS)
                {
                    HandPose.FingerPoses.RemoveRange(HandPose.FingerPoses.Count - 1, HandPose.FingerPoses.Count - NUM_FINGERS);
                }
            }
            
            HandPose.FingerPoses[THUMB].Finger = _fingers[THUMB];
            HandPose.FingerPoses[INDEX].Finger = _fingers[INDEX];
            HandPose.FingerPoses[MIDDLE].Finger = _fingers[MIDDLE];
            HandPose.FingerPoses[RING].Finger = _fingers[RING];
            HandPose.FingerPoses[PINKY].Finger = _fingers[PINKY];
        }
    }
}
