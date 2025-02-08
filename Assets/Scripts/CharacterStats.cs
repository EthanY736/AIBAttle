using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public int health;
    public float attackRange = 2f;
    public int attackDamage;
    public int attackCooldown;

    private NavMeshAgent agent;
    private CharacterStats targetEnemy;
    private float nextAttackTime = 0f;
    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponent<Renderer>();

        // Assign random whole-number stats
        health = Random.Range(50, 151); // 50 to 150
        attackDamage = Random.Range(5, 21); // 5 to 20
        attackCooldown = Random.Range(1, 4); // 1 to 3
        agent.speed = Random.Range(3, 8); // 3 to 7

        // Assign a random color that is not red
        originalColor = GetRandomNonRedColor();
        enemyRenderer.material.color = originalColor;

        FindNewTarget();
    }

    void Update()
    {
        if (targetEnemy == null || targetEnemy.health <= 0)
        {
            FindNewTarget();
        }
        else
        {
            ChaseAndAttackTarget();
        }
    }

    void FindNewTarget()
    {
        CharacterStats[] enemies = FindObjectsOfType<CharacterStats>();
        float closestDistance = Mathf.Infinity;
        CharacterStats closestEnemy = null;

        foreach (CharacterStats enemy in enemies)
        {
            if (enemy != this && enemy.health > 0)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        targetEnemy = closestEnemy;
    }

    void ChaseAndAttackTarget()
    {
        if (targetEnemy == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.transform.position);

        if (distanceToTarget > attackRange)
        {
            agent.SetDestination(targetEnemy.transform.position);
        }
        else
        {
            agent.ResetPath();
            AttackTarget();
        }
    }

    void AttackTarget()
    {
        if (Time.time >= nextAttackTime && targetEnemy.health > 0)
        {
            targetEnemy.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= (int)damage;
        FlashRed();
        if (health <= 0)
        {
            Die();
        }
    }

    void FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        Invoke("ResetColor", 0.2f);
    }

    void ResetColor()
    {
        enemyRenderer.material.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    Color GetRandomNonRedColor()
    {
        Color color;
        do
        {
            color = new Color(Random.value, Random.value, Random.value);
        }
        while (color.r > 0.7f && color.g < 0.5f && color.b < 0.5f); // Avoid strong reds
        return color;
    }
}
