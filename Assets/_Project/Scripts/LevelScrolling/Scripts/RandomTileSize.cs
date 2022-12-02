using UnityEngine;

public class RandomTileSize : MonoBehaviour
{
    public int minSize;
    public int maxSize;

    public void RandomizeSize()
    {
        int randomIndex = 0;
        int randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, transform.transform.localScale.y, randomSize);
    }
}
