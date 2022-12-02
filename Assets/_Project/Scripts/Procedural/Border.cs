using System;
using UnityEngine;

namespace PaperBoy.ObjectPool
{
    public class Border : PoolableObject
    {
        private void OnEnable()
        {
            bool isOnRightSide = ThisTransform.forward == Vector3.left;
            if (isOnRightSide)
            {
                Vector3 localScale = ThisTransform.localScale;
                localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                ThisTransform.localScale = localScale;
            }
        }
    }
}
