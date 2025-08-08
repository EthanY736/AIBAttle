using System.Transactions;
using UnityEngine;
using System.Collections;


public class MeleeFighter : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public int damage = 20;
    public int armor = 0;
    public float moveSpeed = 3f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    public float detectionRange = 10f;
    public float coneAngle = 45f;
    public int rayCount = 10;
    public float attackRange = 2f;
    public float HealthRegenOnKill;
    public float regenInterval = 10f;
    public int regenAmount = 5;
    public int kills;
    private bool isRegenerating = false;
    private Transform target;
    public bool inFightMode = false;
    private Renderer enemyRenderer;
    private Color originalColor;
    public bool seekingArmor = false;
    public bool seekingWeapon = false;
    public static event System.Action<GameObject> OnEnemyGotKill;


    public static MeleeFighter instance;
    public static event System.Action<GameObject, GameObject> OnEnemyDied;


    private GameObject lastAttacker;








    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        maxHealth = Random.Range(80, 120);
        currentHealth = maxHealth;
        damage = Random.Range(5, 10);
        attackCooldown = Random.Range(1, 2);
        moveSpeed = Random.Range(3, 6);


        lastAttackTime = -attackCooldown;
        enemyRenderer = GetComponent<Renderer>();
        originalColor = GetRandomNonRedColor();
        enemyRenderer.material.color = originalColor;
        //HealthRegenOnKill = maxHealth / 4;
    }


    void Update()
    {
        if (inFightMode && target != null)
        {
            MoveTowardsTarget();
            StopCoroutine(RegenOverTime());


            if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                DealDamage(target.gameObject);
                lastAttackTime = Time.time;
            }


            if (target.GetComponent<MeleeFighter>().currentHealth <= 0)
            {
                target = null;
                inFightMode = false;
                Heal();
                kills = kills + 1;
                OnEnemyGotKill?.Invoke(gameObject);
            }
        }
        else if (seekingArmor && target != null)
        {
            MoveTowardsTarget();


            if (Vector3.Distance(transform.position, target.position) <= attackRange * 2)
            {
                ShieldCore shieldCore = target.GetComponent<ShieldCore>();


                if (shieldCore == null)
                {
                    Debug.LogWarning("No ShieldCore found on target.");
                }
                else
                {
                    int bonus = shieldCore.GetBonusProtection();
                    if (bonus > 0)
                    {
                        ApplyShieldStats(shieldCore);
                        Destroy(target.gameObject);
                        ClearSearch();
                    }
                    else
                    {
                        Debug.LogWarning("ShieldCore or selected weapon not properly set. Bonus: " + bonus);
                    }
                }
            }
        }
        else if (seekingWeapon && target != null)
        {
            MoveTowardsTarget();


            if (Vector3.Distance(transform.position, target.position) <= attackRange * 2)
            {
                WeaponCore weaponCore = target.GetComponent<WeaponCore>();


                if (weaponCore == null)
                {
                    Debug.LogWarning("No WeaponCore found on target.");
                }
                else
                {
                    int bonus = weaponCore.GetBonusDamage();
                    if (bonus > 0)
                    {
                        Debug.Log($"Applying weapon bonus: {bonus}");
                        ApplyWeaponStats(weaponCore);
                        Destroy(target.gameObject);
                        ClearSearch();
                    }
                    else
                    {
                        Debug.LogWarning("WeaponCore or selected weapon not properly set. Bonus: " + bonus);
                    }
                }
            }
        }
        else
        {
            ShootRaycasts();
            StartHealthRegen();
        }
    }


    void ApplyWeaponStats(WeaponCore core)
    {
        int bonus = core.GetBonusDamage();
        damage += bonus;
    }
    void ApplyShieldStats(ShieldCore core)
    {
        int bonus = core.GetBonusProtection();
        armor += bonus;
    }


    void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }


    void ShootRaycasts()
    {
        float angleStep = coneAngle / (rayCount - 1);
        float startAngle = -coneAngle / 2;
        Vector3 rayOrigin = transform.position + Vector3.up * -.85f;


        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;


            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, detectionRange))
            {
                Debug.DrawRay(rayOrigin, direction * hit.distance, Color.red);


                if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject != gameObject)
                    EnterFightMode(hit.collider.transform);
                if (hit.collider.CompareTag("Armour"))
                    PickUpArmor(hit.collider.transform);
                if (hit.collider.CompareTag("Weapon"))
                    PickUpWeapon(hit.collider.transform);
            }
            else
            {
                Debug.DrawRay(rayOrigin, direction * detectionRange, Color.green);
            }
        }
    }


    void PickUpArmor(Transform armorObject)
    {
        target = armorObject;
        seekingArmor = true;
        seekingWeapon = false;
        inFightMode = false;
    }


    void PickUpWeapon(Transform weaponObject)
    {
        target = weaponObject;
        seekingWeapon = true;
        seekingArmor = false;
        inFightMode = false;
    }


    void EnterFightMode(Transform enemy)
    {
        target = enemy;
        inFightMode = true;
        seekingArmor = false;
        seekingWeapon = false;
    }


    void ClearSearch()
    {
        target = null;
        inFightMode = false;
        seekingArmor = false;
        seekingWeapon = false;
    }


    void DealDamage(GameObject enemy)
    {
        MeleeFighter enemyScript = enemy.GetComponent<MeleeFighter>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage, gameObject);
        }
    }


    public void TakeDamage(int amount, GameObject attacker = null)
    {
        if (attacker != null)
            lastAttacker = attacker;


        int effectiveDamage = Mathf.Max(amount - armor / 10, 0);
        currentHealth -= effectiveDamage;
        Debug.Log("Health: " + currentHealth);
        FlashRed();


        if (currentHealth <= 0)
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
        OnEnemyDied?.Invoke(gameObject, lastAttacker);
        Destroy(gameObject);
    }




    public void Heal()
    {
        currentHealth += HealthRegenOnKill;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }


    public void StartHealthRegen()
    {
        if (!isRegenerating)
            StartCoroutine(RegenOverTime());
    }
    IEnumerator RegenOverTime()
    {
        isRegenerating = true;


        while (currentHealth < maxHealth)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            yield return new WaitForSeconds(regenInterval);
        }


        isRegenerating = false;
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


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            TakeDamage(10);
        }
    }
}



