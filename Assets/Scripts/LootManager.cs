using UnityEngine;
using Zenject;

public class LootManager : MonoBehaviour
{
    public GameObject experienceGemPrefab;
    public GameObject healthPotionPrefab;
    public GameObject ammoBoxPrefab;
    public Transform spawnPoint; 

    public float spawnRadius = 1.0f;

    public void SpawnLoot(Vector3 position)
    {
        LootType lootType = (LootType)Random.Range(0, 3);
        GameObject lootPrefab = null;

        switch (lootType)
        {
            case LootType.ExperienceGem:
                lootPrefab = experienceGemPrefab;
                break;
            case LootType.HealthPotion:
                lootPrefab = healthPotionPrefab;
                break;
            case LootType.AmmoBox:
                lootPrefab = ammoBoxPrefab;
                break;
        }

        if (lootPrefab != null)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                0f);
            
            Vector3 lootPosition = position + randomOffset;
            GameObject lootObject = Instantiate(lootPrefab, lootPosition, Quaternion.identity, spawnPoint);
        }
    }
}