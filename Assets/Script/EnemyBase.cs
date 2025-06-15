using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float health = 20f;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float chaseRange = 5f; // Khoảng cách để đuổi theo
    [SerializeField] protected GameObject dropPrefab; // Vật phẩm drop (nếu có)

    protected Transform player;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer missing on Enemy!");
        }
        if (player == null)
        {
            Debug.LogWarning("No Player with tag 'Player' found!");
        }
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
        }
    }

    protected virtual void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        spriteRenderer.flipX = direction.x < 0; // Lật sprite theo hướng
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arrow"))
        {
            Arrow arrow = other.GetComponent<Arrow>();
            if (arrow != null)
            {
                TakeDamage(arrow.GetDamage()); // Sử dụng phương thức GetDamage
                Arrow.ReturnArrow(arrow); // Trả arrow về pool
            }
        }
    }

    protected virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}