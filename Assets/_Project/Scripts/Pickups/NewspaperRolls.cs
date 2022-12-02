using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperRolls : MonoBehaviour
{
    [SerializeField] private GameObject[] newspaperRolls;
    public int newspaperRollsIndex;

    public void AddNewspaperRollToBasket()
    {
        if (newspaperRollsIndex < 12)
        {
            newspaperRolls[newspaperRollsIndex].SetActive(true);
            newspaperRollsIndex++;
        }
    }
}
