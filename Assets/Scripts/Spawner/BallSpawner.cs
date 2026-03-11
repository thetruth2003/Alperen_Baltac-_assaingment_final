using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnInterval = 2f;
    public int maxBalls = 20;

    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private float timer;
    private int spawnedBallCount;

    void Update()
    {
        // Stop if prefab or spawn points are missing
        if (ballPrefab == null || spawnPoint1 == null || spawnPoint2 == null)
            return;

        timer += Time.deltaTime;

        // Spawn a ball after the timer reaches the interval
        if (timer >= spawnInterval && spawnedBallCount < maxBalls)
        {
            SpawnBall();
            timer = 0f;
        }
    }

    void SpawnBall()
    {
        // Pick one of the two spawn points randomly
        Transform selectedPoint = Random.value < 0.5f ? spawnPoint1 : spawnPoint2;

        Instantiate(ballPrefab, selectedPoint.position, Quaternion.identity);
        spawnedBallCount++;
    }
}