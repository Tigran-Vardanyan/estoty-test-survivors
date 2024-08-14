using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemyPrefabs;
    public Player _player;
    public float spawnRate = 2.0f;
    private float nextSpawnTime;
    private Camera mainCamera;
   private LootManager lootManager;

    [Inject]
    public void Construct(LootManager _lootManager )
    {
        lootManager = _lootManager;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Please ensure the camera is tagged as 'MainCamera'.");
            return;
        }

        nextSpawnTime = Time.time + spawnRate;
    }

    private void Update()
    {
        if (mainCamera == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
            // Increase spawn rate over time
            spawnRate *= 0.98f; 
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Enemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        newEnemy.Construct(_player);
        newEnemy.lootManager = lootManager;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        // Define how far outside the camera view the enemies should spawn
        float distanceOutsideViewport = 0.1f; 

        // Randomly select a side to spawn the enemy from
        float screenX = 0f;
        float screenY = 0f;

        // Determine a random edge of the screen to spawn the enemy
        switch (Random.Range(0, 4))
        {
            case 0: // Top
                screenX = Random.Range(0f, 1f);
                screenY = 1f + distanceOutsideViewport;
                break;
            case 1: // Bottom
                screenX = Random.Range(0f, 1f);
                screenY = 0f - distanceOutsideViewport;
                break;
            case 2: // Right
                screenX = 1f + distanceOutsideViewport;
                screenY = Random.Range(0f, 1f);
                break;
            case 3: // Left
                screenX = 0f - distanceOutsideViewport;
                screenY = Random.Range(0f, 1f);
                break;
        }

       
        spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(screenX, screenY, mainCamera.farClipPlane));
        spawnPosition.z = 0f;

        // Additional check to ensure the enemy doesn't spawn on top of the player
        if (Vector3.Distance(spawnPosition, _player.transform.position) < 1f) 
        {
            return GetRandomSpawnPosition(); // Recursively find a new position if too close to the player
        }

        return spawnPosition;
    }
}