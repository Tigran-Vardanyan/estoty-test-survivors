using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeManager : MonoBehaviour
{
    private int currentExperience = 0;  
    private int currentLevel = 0;       
    
    [SerializeField] private Player player;       
    [SerializeField] private UIManager uiManager; 

    public enum UpgradeType
    {
        ShootRateIncrease,
        MaxHealthIncrease,
        MoveSpeedIncrease,
        PoisonedBullets
    }

    public class Upgrade
    {
        public UpgradeType upgradeType;
        public string upgradeDescription;
        public float value;
        [SerializeField] private UIManager _uiManager;

        public Upgrade(UpgradeType upgradeType, string upgradeDescription, float value)
        {
            this.upgradeType = upgradeType;
            this.upgradeDescription = upgradeDescription;
            this.value = value;
        }

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

    private List<Upgrade> availableUpgrades = new List<Upgrade>
    {
        new Upgrade(UpgradeType.ShootRateIncrease, "Shoot Rate Increased!", 0.2f),
        new Upgrade(UpgradeType.MaxHealthIncrease, "Max Health Increased!", 20f),
        new Upgrade(UpgradeType.MoveSpeedIncrease, "Move Speed Increased!", 0.5f),
        new Upgrade(UpgradeType.PoisonedBullets, "Poisoned Bullets Unlocked!", 0f) 
    };

    [Inject]
    public void Construct(Player player)
    {
        this.player = player;
    }

    
    public void ApplyRandomUpgrade()
    {
        if (availableUpgrades.Count == 0) return;

        int randomIndex = Random.Range(0, availableUpgrades.Count);
        Upgrade selectedUpgrade = availableUpgrades[randomIndex];
        selectedUpgrade.ApplyUpgrade(player);

        uiManager.ShowUpgradeMessage(selectedUpgrade.upgradeDescription);
    }
}