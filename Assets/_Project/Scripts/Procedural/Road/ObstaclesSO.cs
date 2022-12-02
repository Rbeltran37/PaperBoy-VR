using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesSO", menuName = "ScriptableObjects/PaperBoy/Obstacles/ObstaclesSO", order = 1)]
public class ObstaclesSO : ScriptableObject
{
    public List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] private List<float> oddsOfSpawningPerObject = new List<float>();
    
    [Space(10)]
    [SerializeField] private float oddsOfSpawningObject;
    [SerializeField] private int minSpawnAttemptCount;
    [SerializeField] private int maxSpawnAttemptCount;

    public GameObject GetObstacle()
    {
        if (obstacles.Count == 0) return null;

        var randomRoll = Random.Range(0, 100);
        if (oddsOfSpawningObject > randomRoll)
        {
            var randomIndex = IndexBasedOffOdds(oddsOfSpawningPerObject);
            return obstacles[randomIndex];
        }

        return null;
    }

    public int GetSpawnAttemptCount()
    {
        return Random.Range(minSpawnAttemptCount, maxSpawnAttemptCount+1);
    }
    
    private int IndexBasedOffOdds (List<float> probabilities) 
    {
        float total = 0;

        foreach (float elem in probabilities) {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i= 0; i < probabilities.Count; i++) {
            if (randomPoint < probabilities[i]) {
                return i;
            }
            else {
                randomPoint -= probabilities[i];
            }
        }
        return probabilities.Count;
    }
}
