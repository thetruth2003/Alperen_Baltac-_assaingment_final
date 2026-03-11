using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private List<Transform> _spawnPoints;

    private void Awake()
    {
        Instance = this;
        Debug.Log("SpawnManager initialized. SpawnPoints: " + _spawnPoints.Count);
    }

    public Transform GetRandomSpawnPoint()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0)
        {
            Debug.LogError("Spawn points are not set or empty.");
            return null;
        }

        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }
}