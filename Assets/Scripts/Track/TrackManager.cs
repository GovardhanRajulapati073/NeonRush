
using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    [Header("Track Settings")]
    public Transform player;
    public GameObject[] trackPrefabs;
    public float sectionLength = 30f;
    public int initialSections = 6;
    public float spawnAhead = 90f;
    public float despawnBehind = 35f;

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public bool spawnObstacles = true;
    public int obstacleEverySections = 1;

    [Header("PowerUp Settings")]
    public GameObject speedBoostPrefab;
    public bool spawnSpeedBoost = true;
    public int speedBoostEverySections = 3;

    private readonly Queue<GameObject> activeSections =
        new Queue<GameObject>();

    private float nextSpawnZ;
    private int sectionCount;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("TrackManager: Assign the Player.");
            enabled = false;
            return;
        }

        if (trackPrefabs == null || trackPrefabs.Length == 0)
        {
            Debug.LogError("TrackManager: Assign a track prefab.");
            enabled = false;
            return;
        }

        if (spawnObstacles && obstaclePrefab == null)
        {
            Debug.LogError("TrackManager: Assign the Barrier prefab.");
            enabled = false;
            return;
        }

        for (int i = 0; i < initialSections; i++)
        {
            SpawnSection();
        }
    }

    void Update()
    {
        if (player == null)
            return;

        while (nextSpawnZ < player.position.z + spawnAhead)
        {
            SpawnSection();
        }

        while (activeSections.Count > 0)
        {
            GameObject oldest = activeSections.Peek();

            if (player.position.z <
                oldest.transform.position.z +
                sectionLength + despawnBehind)
            {
                break;
            }

            Destroy(activeSections.Dequeue());
        }
    }

    void SpawnSection()
    {
        int index = Random.Range(0, trackPrefabs.Length);

        GameObject section = Instantiate(
            trackPrefabs[index],
            new Vector3(0f, 0f, nextSpawnZ),
            Quaternion.identity
        );

        activeSections.Enqueue(section);

        // Spawn obstacles
        if (spawnObstacles &&
            sectionCount % Mathf.Max(1, obstacleEverySections) == 0)
        {
            SpawnObstacle(nextSpawnZ);
        }

        // Spawn SpeedBoost power-ups
        if (spawnSpeedBoost &&
            speedBoostPrefab != null &&
            sectionCount % Mathf.Max(1, speedBoostEverySections) == 0)
        {
            SpawnSpeedBoost(nextSpawnZ);
        }

        nextSpawnZ += sectionLength;
        sectionCount++;
    }

    void SpawnObstacle(float sectionStartZ)
    {
        float[] lanePositions = { -3f, 0f, 3f };

        int laneIndex = Random.Range(0, lanePositions.Length);

        Vector3 obstaclePosition = new Vector3(
            lanePositions[laneIndex],
            0.6f,
            sectionStartZ + 15f
        );

        Instantiate(
            obstaclePrefab,
            obstaclePosition,
            Quaternion.identity
        );
    }

    void SpawnSpeedBoost(float sectionStartZ)
    {
        float[] lanePositions = { -3f, 0f, 3f };

        int laneIndex = Random.Range(0, lanePositions.Length);

        // Updated SpeedBoost position
        Vector3 powerUpPosition = new Vector3(
            lanePositions[laneIndex],
            1.2f,
            sectionStartZ + 25f
        );

        Instantiate(
            speedBoostPrefab,
            powerUpPosition,
            Quaternion.identity
        );
    }
}
