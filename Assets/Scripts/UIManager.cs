using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider experienceBar;
    public Slider healthBar;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI killsText;
    public GameObject deathScreen;
    public TextMeshProUGUI upgradeMessageText; // Add this for upgrade messages

    [Header("Player Settings")]
    public Player player;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set in UIManager.");
        }
        UpdateUI();
        deathScreen.SetActive(false);
        upgradeMessageText.gameObject.SetActive(false); // Hide upgrade message by default
    }

    private void Update()
    {
        // Update UI in real-time
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (player != null)
        {
            // Update Experience Bar
            UpdateExperienceBar();

            // Update Health Bar
            healthBar.value = player.GetHealthNormalized();

            // Update Ammo Text
            ammoText.text = player.CurrentAmmo + "/" + player.maxAmmo;

            // Update Kills Counter
            killsText.text = player.KillsCount.ToString(); 
        }
    }

    public void UpdateExperienceBar()
    {
        if (player != null)
        {
            experienceBar.value = player.GetExperienceNormalized(); // Assuming you have this method
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void ShowUpgradeMessage(string message)
    {
        if (upgradeMessageText != null)
        {
            upgradeMessageText.text = message;
            upgradeMessageText.gameObject.SetActive(true);
            Invoke("HideUpgradeMessage", 2f); // Hides the message
        }
    }

    private void HideUpgradeMessage()
    {
        if (upgradeMessageText != null)
        {
            upgradeMessageText.gameObject.SetActive(false);
        }
    }
}