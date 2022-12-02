using System;
using UnityEngine;

namespace PaperBoy.Hands
{
    public class HandAnimator : MonoBehaviour
    {
        [SerializeField] private Grab grab;
        [SerializeField] private HandPoseSO idleHandPoseSO;
        [SerializeField] private HandPoseSO grabHandPoseSO;
        [SerializeField] private Transform palm;
        [SerializeField] private Transform[] fingers;
        

        private void OnValidate()
        {
            if (palm)
            {
                fingers = new Transform[HandPoseSO.NUM_FINGERS];
                int childCount = palm.childCount;
                for (int i = 0; i < childCount && i < HandPoseSO.NUM_FINGERS; i++)
                {
                    fingers[i] = palm.GetChild(i);
                }
            }

            if (!grab)
            {
                grab = GetComponentInChildren<Grab>();
            }
        }

        private void Awake()
        {
            grab.GrabbedNewspaper += Grab;
            grab.LaunchedNewspaper += Idle;
            
            Idle();
        }

        public void Idle()
        {
            SetPose(idleHandPoseSO);
        }

        public void Grab()
        {
            SetPose(grabHandPoseSO);
        }

        private void SetPose(HandPoseSO handPoseSO)
        {
            for (int i = 0; i < HandPoseSO.NUM_FINGERS; i++)
            {
                Transform jointTransform = fingers[i];
                for (int j = 0; j < FingerPose.NUM_JOINTS; j++)
                {
                    jointTransform.localPosition = handPoseSO.HandPose.FingerPoses[i].Joints[j].Position;
                    jointTransform.localRotation = handPoseSO.HandPose.FingerPoses[i].Joints[j].Rotation;

                    if (jointTransform.childCount == 0)
                    {
                        break;
                    }
                    
                    jointTransform = jointTransform.GetChild(0);
                }
            }
        }
    }
}
