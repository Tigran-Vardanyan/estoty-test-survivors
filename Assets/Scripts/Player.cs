using System.Linq;
using UnityEngine;
using Zenject; // For Dependency Injection

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public int maxAmmo = 30;
    public float shootingRange = 10f;
    public float shootDelay = 0.2f;
    public LayerMask enemyLayer;
    public float lootAttractionRange = 3f;

    [Header("References")]
    public Joystick joystick;
    public Camera mainCamera;
    public Transform gunTransform;
    public Transform lootAttractorPoint;
    [SerializeField] private Bullet bulletPref;

    private int currentAmmo;
    private float currentHealth;
    private float maxHealth = 100f; // Set initial maxHealth or load from player stats
    private float shootRate = 0.2f; // Initial shoot rate
    private bool hasPoisonedBullets = false;
    private Rigidbody2D rb;
    private EnemyManager enemyManager;
    private UpgradeManager upgradeManager;
    private LootManager lootManager;

    // Experience fields
    private int experience;
    private int experienceToNextLevel = 100; // Set initial value or make configurable
    public int Level { get; private set; } = 1;
    public int CurrentAmmo => currentAmmo; // Property to access current ammo
    public int KillsCount { get; private set; } // Property to track kills

    [Inject]
    public void Construct(EnemyManager enemyManager, UpgradeManager upgradeManager, LootManager lootManager)
    {
        this.enemyManager = enemyManager;
        this.upgradeManager = upgradeManager;
        this.lootManager = lootManager;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        AttractLoot();
    }

    void HandleMovement()
    {
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 moveVelocity = moveInput.normalized * moveSpeed;
        rb.velocity = moveVelocity;

        // Camera follows the player
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
    }

    void HandleShooting()
    {
        if (currentAmmo <= 0) return;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, shootingRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            Collider2D closestEnemy = enemiesInRange.OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).FirstOrDefault();

            if (closestEnemy != null)
            {
                AimAndShoot(closestEnemy.transform);
            }
        }
    }

    void AimAndShoot(Transform target)
    {
        Vector2 direction = (target.position - gunTransform.position).normalized;
        gunTransform.right = direction;

        // Implement shooting logic
        Shoot();
    }

    void Shoot()
    {
        if (Time.time < shootRate) return;

        // Check if there's ammo
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        // Decrease ammo
        currentAmmo--;
        
        // Instantiate the bullet at the gun's position and rotation
        GameObject bullet = Instantiate(bulletPref.gameObject, gunTransform.position,gunTransform.rotation);

        // Add force to the bullet in the direction the gun is facing
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = gunTransform.right * bulletPref.speed;
        }

        // Update the shoot rate to limit the fire rate
        shootRate = Time.time + shootDelay;

        // Log the shot for debugging purposes
        Debug.Log("Shot fired! Remaining ammo: " + currentAmmo);
    
    }

    void AttractLoot()
    {
        Collider2D[] lootInRange = Physics2D.OverlapCircleAll(transform.position, lootAttractionRange);

        foreach (var loot in lootInRange)
        {
            if (loot.CompareTag("Loot"))
            {
                loot.transform.position = Vector2.MoveTowards(loot.transform.position, lootAttractorPoint.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the shooting range and loot attraction range for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lootAttractionRange);
    }

    public void ApplyLoot(Loot loot)
    {
        switch (loot.lootType)
        {
            case LootType.ExperienceGem:
                AddExperience(loot.value);
                break;
            case LootType.HealthPotion:
                RestoreHealth(loot.value);
                break;
            case LootType.AmmoBox:
                RestoreAmmo(loot.value);
                break;
        }
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.ShootRateIncrease:
                IncreaseShootRate(upgrade.value);
                break;
            case UpgradeType.MaxHealthIncrease:
                IncreaseMaxHealth(upgrade.value);
                break;
            case UpgradeType.MoveSpeedIncrease:
                IncreaseMoveSpeed(upgrade.value);
                break;
            case UpgradeType.PoisonedBullets:
                ApplyPoisonedBullets();
                break;
        }
        Debug.Log("Received Upgrade: " + upgrade.upgradeType);
    }

    public void AddExperience(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
        
        // Notify UIManager to update experience bar
        FindObjectOfType<UIManager>().UpdateUI();
    }

    private void LevelUp()
    {
        experience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f); // Increase difficulty
        Level++;

        // Notify UIManager about level-up or trigger upgrade
        FindObjectOfType<UIManager>().UpdateUI();
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Optionally update health UI
        FindObjectOfType<UIManager>().UpdateUI();
    }

    public void RestoreAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        // Optionally update ammo UI
        FindObjectOfType<UIManager>().UpdateUI();
    }

    public void IncreaseShootRate(float amount)
    {
        shootRate = Mathf.Max(0.1f, shootRate - amount); // Ensure shootRate doesn't go below a threshold
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Optionally fully heal the player
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void ApplyPoisonedBullets()
    {
        hasPoisonedBullets = true;
        // Implement poisoned bullets logic, e.g., affecting enemy colors and applying periodic damage
    }

    public float GetExperienceNormalized()
    {
        return (float)experience / experienceToNextLevel;
    }

    public float GetHealthNormalized()
    {
        return currentHealth / maxHealth;
    }

    public void AddKill()
    {
        KillsCount++;
        // Optionally notify UIManager about kill count
        FindObjectOfType<UIManager>().UpdateUI();
    }

    public void Die()
    {
        // Notify the UIManager or trigger death screen
        FindObjectOfType<UIManager>().ShowDeathScreen();
    }

    public void TakeDamage(int damage)
    {
        
    }
}