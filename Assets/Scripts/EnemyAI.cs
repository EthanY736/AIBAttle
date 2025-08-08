using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeleeFighter))]
public class EnemyAI : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float moveSpeed = 3f;


    private Vector3 targetPosition;
    private MeleeFighter meleeFighter;


    [Header("Fail Safe")]
    public float wanderFailDuration = 5f;
    private float wanderFailTimer;


    [Header("Zone Settings")]
    public string zoneExitTag = "ExitZone";
    private Transform zoneExitPoint;
    private bool inZone = false;


    private static readonly string[] firstNames = {
        "Alex", "Jordan", "Taylor", "Casey", "Riley",
        "Morgan", "Jamie", "Logan", "Reese", "Cameron"
    };


    private static readonly string[] lastNames = {
        "Stone", "Black", "Miller", "Hayes", "Turner",
        "Brooks", "West", "Knight", "Reed", "Dawson"
    };


    void Awake()
    {
        string randomFirst = firstNames[Random.Range(0, firstNames.Length)];
        string randomLast = lastNames[Random.Range(0, lastNames.Length)];
        gameObject.name = $"{randomFirst} {randomLast}";
    }


    void Start()
    {
        meleeFighter = GetComponent<MeleeFighter>();
        PickNewWanderPosition();
    }


    void Update()
    {
        if (inZone && zoneExitPoint != null)
        {
            MoveTo(zoneExitPoint.position);
        }
        else if (!meleeFighter.inFightMode && !meleeFighter.seekingArmor && !meleeFighter.seekingWeapon)
        {
            wanderFailTimer += Time.deltaTime;


            bool reached = MoveTo(targetPosition);
            if (reached || wanderFailTimer >= wanderFailDuration)
            {
                PickNewWanderPosition();
            }
        }
    }


    void PickNewWanderPosition()
    {
        Vector2 random2D = Random.insideUnitCircle * wanderRadius;
        Vector3 flatOffset = new Vector3(random2D.x, 0f, random2D.y);
        targetPosition = transform.position + flatOffset;
        targetPosition.y = transform.position.y;


        wanderFailTimer = 0f; // reset fail-safe timer
    }


    // Returns true if destination is reached this frame
    bool MoveTo(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        destination.y = startPosition.y;


        Vector3 direction = destination - startPosition;
        float distanceThisFrame = moveSpeed * Time.deltaTime;


        if (direction.sqrMagnitude <= distanceThisFrame * distanceThisFrame)
        {
            transform.position = destination;
            return true;
        }


        Vector3 moveDir = direction.normalized;


        if (moveDir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }


        transform.position = startPosition + moveDir * distanceThisFrame;
        return false;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            inZone = true;
            GameObject exit = GameObject.FindGameObjectWithTag(zoneExitTag);
            if (exit != null)
            {
                zoneExitPoint = exit.transform;
            }
        }
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zone"))
        {
            StartCoroutine(ExitZoneDelay());
        }
    }


    IEnumerator ExitZoneDelay()
    {
        yield return new WaitForSeconds(3f);
        inZone = false;
        zoneExitPoint = null;
        PickNewWanderPosition();
        Debug.Log("Character exited zone, resuming wandering.");
    }
}



