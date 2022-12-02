using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperStackSpawner : MonoBehaviour
{
    private NewspaperStack _newspaperStack;
    [SerializeField] private GameObject newsPaperStack;
    [SerializeField] private int tilesBetweenSpawns;

    private void Awake()
    {
        SpawnNewspaper();
    }

    private void SpawnNewspaper()
    {
        _newspaperStack = transform.parent.GetComponent<NewspaperStack>();
        _newspaperStack.numberOfTilesBetweenSpawns++;

        if (_newspaperStack.numberOfTilesBetweenSpawns == tilesBetweenSpawns)
        {
            newsPaperStack.transform.localPosition = new Vector3 (Random.Range(-0.32f, 0.32f), newsPaperStack.transform.position.y, Random.Range(-0.444f, 0.444f));
            newsPaperStack.SetActive(true);
            _newspaperStack.numberOfTilesBetweenSpawns = 0;
        }
    }
}
