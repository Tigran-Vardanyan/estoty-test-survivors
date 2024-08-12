using System.Collections;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    public float health;
    public int damage;
    public float speed;

    public Sprite[] sprites; // Array of sprites to choose from
    public Sprite damageSprite; // Sprite to change to when taking damage
    public Sprite deathSprite; // Sprite to change to upon death
    public LootManager lootManager;

    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isAttacking;
    

    [Inject]
    public void Construct(Transform playerTransform )
    {
        player = playerTransform;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseRandomSprite();
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void ChooseRandomSprite()
    {
        if (sprites.Length > 0)
        {
            int randomIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randomIndex];
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.position) < 1.5f && !isAttacking)
        {
            StartCoroutine(DealPeriodicDamage());
        }
    }

    private IEnumerator DealPeriodicDamage()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, player.position) < 1.5f)
        {
            
             player.GetComponent<Player>().TakeDamage(damage);

            yield return new WaitForSeconds(1.0f); // Deal damage every second
        }

        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        // Change sprite to damageSprite when taking damage
        if (damageSprite != null)
        {
            spriteRenderer.sprite = damageSprite;
        }

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator  Die()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        // Change sprite to deathSprite upon death
        if (deathSprite != null)
        {
            spriteRenderer.sprite = deathSprite;
        }
       // lootManager.SpawnLoot(gameObject.transform.position);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}