using UnityEngine;public class CharacterStats : MonoBehaviour{    public int maxHealth = 100;    public int currentHealth;    public int damage = 10;    public float moveSpeed = 3f;    private Transform targetEnemy;    public bool IsAttacking = false;    void Start()    {        currentHealth = maxHealth;        FindNearestEnemy();    }    void Update()    {        if (targetEnemy != null)        {            MoveTowardsEnemy();        }    }    public void TakeDamage(int amount)    {        currentHealth -= amount;        if (currentHealth <= 0)        {            Die();        }    }    void Die()    {        Debug.Log("Character has died!");
        // Add death logic here (e.g., respawn, game over)
    }
    void FindNearestEnemy()    {        CharacterStats nearestEnemy = null;        float shortestDistance = Mathf.Infinity;        CharacterStats[] enemies = FindObjectsOfType<CharacterStats>();

        foreach (CharacterStats enemy in enemies)        {            if (enemy != this)            {                float distance = Vector3.Distance(transform.position, enemy.transform.position);                if (distance < shortestDistance)                {                    shortestDistance = distance;                    nearestEnemy = enemy;                }            }        }

        if (nearestEnemy != null)        {            targetEnemy = nearestEnemy.transform;            Debug.Log("Nearest enemy found: " + nearestEnemy.name);        }    }
    void MoveTowardsEnemy()    {        transform.position = Vector3.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);    }    void OnCollisionEnter(Collision collision)    {
        if (collision.gameObject.CompareTag("Enemy"))        {            IsAttacking = true;            CharacterStats enemyStats = collision.gameObject.GetComponent<CharacterStats>();            if (enemyStats != null)            {                enemyStats.TakeDamage(damage);            }        }    }}