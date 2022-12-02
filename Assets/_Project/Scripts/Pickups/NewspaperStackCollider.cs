using UnityEngine;

public class NewspaperStackCollider : MonoBehaviour
{
    [SerializeField] private int numberOfPaperRollToAdd;

    public int NumPapers => numberOfPaperRollToAdd;


    public void Collect()
    {
        gameObject.SetActive(false);
    }
}
