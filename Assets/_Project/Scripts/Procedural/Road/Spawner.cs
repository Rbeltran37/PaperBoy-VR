using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private ObstaclesSO obstacleSO;
    [HideInInspector]
    public bool isFixedPosition;
    [HideInInspector]
    public Transform fixedSpawnTransform;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private void OnEnable()
    {
        var spawnCount = obstacleSO.GetSpawnAttemptCount();
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnObject();
        }
    }

    private void OnDisable()
    {
        for(int i =0; i < spawnedObstacles.Count; i++) 
        {
            var currentObstacle = spawnedObstacles[i];
            spawnedObstacles.Remove(currentObstacle);
            Destroy(currentObstacle);
        }
    }

    private static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void SpawnObject()
    {
        var randomObject = obstacleSO.GetObstacle();
        if (!randomObject) return;

        Vector3 position;
        Quaternion rotation;

        if (isFixedPosition)
        {
            position = fixedSpawnTransform.position;
            rotation = fixedSpawnTransform.rotation;
        }
        else
        {
            position = RandomPointInBounds(collider.bounds);
            rotation = Quaternion.identity;
        }

        var snappedToGroundPosition = GetSnapToGroundPosition(position);
        var spawnedObject = Instantiate(randomObject, snappedToGroundPosition, rotation, transform);
        spawnedObstacles.Add(spawnedObject);
    }

    private Vector3 GetSnapToGroundPosition(Vector3 currentPosition)
    {
        return new Vector3(currentPosition.x, 0f, currentPosition.z);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Spawner))]
public class SpawnerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
 
        Spawner script = (Spawner)target;
 
        // draw checkbox for the bool
        script.isFixedPosition = EditorGUILayout.Toggle("IsFixedPosition", script.isFixedPosition);
        
        if (script.isFixedPosition)
        {
            script.fixedSpawnTransform = EditorGUILayout.ObjectField("Fixed Spawn Transform", script.fixedSpawnTransform, typeof(Transform), true) as Transform;
        }
    }
}
#endif
