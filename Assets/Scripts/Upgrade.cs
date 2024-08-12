using UnityEngine;

public enum UpgradeType
{
    ShootRateIncrease,
    MaxHealthIncrease,
    MoveSpeedIncrease,
    PoisonedBullets
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade")]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public string upgradeDescription;
    public float value;

    public void ApplyUpgrade(Player player)
    {
        switch (upgradeType)
        {
            case UpgradeType.ShootRateIncrease:
                player.IncreaseShootRate(value);
                break;
            case UpgradeType.MaxHealthIncrease:
                player.IncreaseMaxHealth(value);
                break;
            case UpgradeType.MoveSpeedIncrease:
                player.IncreaseMoveSpeed(value);
                break;
            case UpgradeType.PoisonedBullets:
                player.ApplyPoisonedBullets();
                break;
        }
    }
}