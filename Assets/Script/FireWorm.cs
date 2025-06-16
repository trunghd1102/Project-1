using UnityEngine;

public class FireWorm : EnemyBase
{
    [SerializeField] private float attackRange = 1f; // Khoảng cách tấn công
    [SerializeField] private float attackCooldown = 1f; // Thời gian chờ giữa các lần tấn công
    private float lastAttackTime;

    protected override void Update()
    {
        base.Update();
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        // Logic tấn công (VD: gây sát thương cho Player, hiện tại chỉ log)
        Debug.Log("Enemy attacking Player!");
    }
}