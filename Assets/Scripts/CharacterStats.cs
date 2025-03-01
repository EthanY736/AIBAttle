using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public int health;
    public float attackRange = 2f;
    public int baseAttackDamage;
    public int attackCooldown;
    public int armourValue = 0;
    public int weaponPower = 0;
    
    public float detectionRange = 10f;
    public float coneAngle = 45f;
    public int rayCount = 10;
    public LayerMask detectionLayer;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private CharacterStats targetEnemy;
    private float nextAttackTime = 0f;
    private Renderer enemyRenderer;
    private Color originalColor;
    private bool searchingForItem = false;
    private float wanderTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRenderer = GetComponent<Renderer>();

        health = Random.Range(50, 151);
        baseAttackDamage = Random.Range(5, 21);
        attackCooldown = Random.Range(1, 4);
        agent.speed = Random.Range(3, 8);

        originalColor = GetRandomNonRedColor();
        enemyRenderer.material.color = originalColor;
        wanderTime = wanderTimer;
    }

    void Update()
    {
        if (searchingForItem) return;

        if (targetEnemy == null || targetEnemy.health <= 0)
        {
            DetectObjectsWithRaycast();
            if (targetEnemy == null)
            {
                Wander();
            }
        }
        else
        {
            ChaseAndAttackTarget();
        }
    }

    void DetectObjectsWithRaycast()
    {
        float angleStep = coneAngle / (rayCount - 1);
        float startAngle = -coneAngle / 2;
        bool foundTarget = false;
        
        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, detectionRange, detectionLayer))
            {
                CharacterStats detectedEnemy = hit.collider.GetComponent<CharacterStats>();
                if (detectedEnemy != null && detectedEnemy != this && detectedEnemy.health > 0)
                {
                    targetEnemy = detectedEnemy;
                    foundTarget = true;
                    break;
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }
        
        if (!foundTarget)
        {
            targetEnemy = null;
        }
    }

    void Wander()
    {
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTime = wanderTimer;
        }
    }

    static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
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
        if (Time.time >= nextAttackTime && targetEnemy != null && targetEnemy.health > 0)
        {
            int effectiveDamage = (int)((1 + (weaponPower / 10f)) * baseAttackDamage * (1 - (targetEnemy.armourValue / 10f)));
            targetEnemy.TakeDamage(effectiveDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= Mathf.Max(1, (int)damage - armourValue);
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
        while (color.r > 0.7f && color.g < 0.5f && color.b < 0.5f);
        return color;
    }
}
