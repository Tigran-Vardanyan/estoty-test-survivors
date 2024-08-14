using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Player,
        Enemy
    }

    public BulletType bulletType; // Assign this in the inspector or through code
    public float speed = 10f;
    public int damage = 10;
    public int destructionTime = 10;

    private void Start()
    {
        StartCoroutine(DelayDestroy((float)destructionTime));
    }

    IEnumerator DelayDestroy(float value)
    {
        yield return new WaitForSeconds(value);
        Destroy(gameObject);
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision based on bullet type
        if (bulletType == BulletType.Player && other.gameObject.layer == LayerMask.NameToLayer ("Enemy"))
        {
            // Assume the enemy has a method to take damage
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (bulletType == BulletType.Enemy && other.CompareTag("Player"))
        {
            // Assume the player has a method to take damage
            other.GetComponent<Player>().TakeDamage(damage);
        }else if (bulletType == BulletType.Player && other.CompareTag("Player")||(bulletType == BulletType.Enemy && other.gameObject.layer == LayerMask.NameToLayer ("Enemy")))
        {
            return;
        }
        Destroy(gameObject);
    }
}