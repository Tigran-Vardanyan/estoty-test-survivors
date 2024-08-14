using System.Collections;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    public float health;
    public int damage;
    public float speed;

    public Sprite[] sprites;
    public Sprite damageSprite;
    public Sprite deathSprite; 
    public LootManager lootManager;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Player _player;
    private bool isAttacking;
    

    [Inject]
    public void Construct(Player player )
    {
        _player = player;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
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
        if (health <= 0) return;
            if ( _player == null) return;

            Vector3 direction = ( _player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            if (direction.x<0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (Vector3.Distance(transform.position,  _player.transform.position) < 1.5f && !isAttacking)
            {
                StartCoroutine(DealPeriodicDamage());
            }
    }

    private IEnumerator DealPeriodicDamage()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position,  _player.transform.position) < 1.5f)
        {
            
            _player.GetComponent<Player>().TakeDamage(damage);

            yield return new WaitForSeconds(1.0f); 
        }

        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
         StartCoroutine(TakingDamagEffrct());
    }

    private IEnumerator TakingDamagEffrct()
    {
        if (damageSprite != null && health >= 0)
        {
            animator.enabled = false;
            spriteRenderer.sprite = damageSprite;
            yield return new WaitForSeconds(0.1f);
            animator.enabled = true;
        }
        else
        {
            StartCoroutine(Die());
        }
       
    }

    private IEnumerator Die()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        // Change sprite to deathSprite upon death
        if (deathSprite != null)
        {
            animator.enabled = false;
            spriteRenderer.sprite = deathSprite;
        }

        lootManager.SpawnLoot(gameObject.transform.position);
        yield return new WaitForSeconds(1);
        _player.AddKill();
        Destroy(gameObject);
    }
}