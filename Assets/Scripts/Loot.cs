using UnityEngine;

public class Loot : MonoBehaviour
{
    public LootType lootType;
    public int value;

    public void ApplyLoot(Player player)
    {
        switch (lootType)
        {
            case LootType.ExperienceGem:
                player.AddExperience(value);
                break;
            case LootType.HealthPotion:
                player.RestoreHealth(value);
                break;
            case LootType.AmmoBox:
                player.RestoreAmmo(value);
                break;
        }
    }
}public enum LootType
 {
     ExperienceGem,
     HealthPotion,
     AmmoBox
 }