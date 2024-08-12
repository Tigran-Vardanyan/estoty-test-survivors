using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> availableUpgrades;
    public int experiencePerLevel = 100;

    private int currentExperience = 0;
    private int currentLevel = 0;
    private Player player;
    private UIManager uiManager;

    [Inject]
    public void Construct(Player playerController, UIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        if (currentExperience >= experiencePerLevel)
        {
            currentExperience -= experiencePerLevel;
            currentLevel++;
            ApplyRandomUpgrade();
        }

        uiManager.UpdateExperienceBar();
    }

    private void ApplyRandomUpgrade()
    {
        if (availableUpgrades.Count == 0) return;

        int randomIndex = Random.Range(0, availableUpgrades.Count);
        Upgrade selectedUpgrade = availableUpgrades[randomIndex];
        selectedUpgrade.ApplyUpgrade(player);

        uiManager.ShowUpgradeMessage(selectedUpgrade.upgradeDescription);
    }
}