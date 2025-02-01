using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public float health = 100f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private CharacterStats targetEnemy;
    private float nextAttackTime = 0f;
    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
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
        health -= damage;
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
}
