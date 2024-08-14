using System;
using UnityEngine;
using Zenject;

public class Loot : MonoBehaviour
{
    public LootType lootType;
    public int value;

    private Player player;

    [Inject]
    public void Construct(Player _player )
    {
        player = _player;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyLoot(player);
        }
    }

    public void ApplyLoot(Player player)
    {
        switch (lootType)
        {
            case LootType.ExperienceGem:
                player.AddExperience(value);
                Destroy(gameObject);
                break;
            case LootType.HealthPotion:
                player.RestoreHealth(value);
                Destroy(gameObject);
                break;
            case LootType.AmmoBox:
                player.RestoreAmmo(value);
                Destroy(gameObject);
                break;
        }
    }
}public enum LootType
 {
     ExperienceGem,
     HealthPotion,
     AmmoBox
 }