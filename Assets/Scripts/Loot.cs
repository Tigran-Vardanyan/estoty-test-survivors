using System;
using UnityEngine;
using Zenject;

public class Loot : MonoBehaviour
{
    public LootType lootType;
    public int value;

    private Player _player;

    [Inject]
    public void Construct(Player player )
    {
        _player = player;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyLoot(_player);
        }
    }

    public void ApplyLoot(Player player)
    {
        switch (lootType)
        {
            case LootType.ExperienceGem:
                _player.AddExperience(value);
                Destroy(gameObject);
                break;
            case LootType.HealthPotion:
                _player.RestoreHealth(value);
                Destroy(gameObject);
                break;
            case LootType.AmmoBox:
                _player.RestoreAmmo(value);
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