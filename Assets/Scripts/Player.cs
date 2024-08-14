using System;
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
    private float maxHealth = 100f; 
    private float shootRate = 0.2f; 
    private bool hasPoisonedBullets = false;
    private Rigidbody2D rb;
    private EnemyManager enemyManager;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UIManager uiManager;
    private LootManager lootManager;

    // Experience fields
    private int experience;
    private int experienceToNextLevel = 10; 
    public int Level { get; private set; } = 1;
    public int CurrentAmmo => currentAmmo; 
    public int KillsCount { get; private set; } 
    

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
        //Flip Player depend on movement directory
        if (moveInput.x<0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        // Camera follows the player
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
    }

    void HandleShooting()
    {
        if (currentAmmo <= 0) return;
        //Find enemy by Layer
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
        Shoot();
    }

    void Shoot()
    {
        if (Time.time < shootRate) return;
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }
        
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
    }

    void AttractLoot()
    {
        Collider2D[] lootInRange = Physics2D.OverlapCircleAll(transform.position, lootAttractionRange);

        foreach (var loot in lootInRange)
        {
            if (loot.CompareTag("Loot"))
            {
                loot.transform.position = Vector2.MoveTowards(loot.transform.position, lootAttractorPoint.position, moveSpeed*1.5f * Time.deltaTime);
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
    

   

    public void AddExperience(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            LevelUp();
            
        }
        uiManager.UpdateUI();
    }

    private void LevelUp()
    {
        experience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f); // Increase difficulty
        Level++;

        upgradeManager.ApplyRandomUpgrade();
        FindObjectOfType<UIManager>().UpdateUI();
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        uiManager.UpdateUI();
    }

    public void RestoreAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        uiManager.UpdateUI();
    }

    public void IncreaseShootRate(float amount)
    {
        shootRate = Mathf.Max(0.1f, shootRate - amount); 
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; 
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void ApplyPoisonedBullets()
    {
        hasPoisonedBullets = true;
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
       
        uiManager.UpdateUI();
    }

    public void Die()
    {
        uiManager.ShowDeathScreen();
        enabled = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        uiManager.UpdateUI();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}