using PaperBoy.Hands;
using UnityEngine;

public class HandPoseTool : MonoBehaviour
{
    [SerializeField] private HandPoseSO handPoseSO;
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
    }
}
