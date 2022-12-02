using System;
using System.Collections.Generic;

namespace PaperBoy.Hands
{
    [Serializable]
    public class HandPose
    {
        public List<FingerPose> FingerPoses = new List<FingerPose>(HandPoseSO.NUM_FINGERS);
    }
}
