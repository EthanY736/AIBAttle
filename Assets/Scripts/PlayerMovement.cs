using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 500f;
    private float rotationY = 0f;
    private float rotationX = 0f;

    public Transform playerCamera;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // stops physics rotation
        rb.linearDamping = 0f;    // we’ll control stopping manually
    }

    void Update()
    {
        // --- Ignore input if an enemy is being controlled ---
        if (EnemyClickTracker.ActiveControlledEnemy != null)
            return;

        // --- Mouse Look ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    void FixedUpdate()
    {
        // --- Ignore movement input if an enemy is being controlled ---
        if (EnemyClickTracker.ActiveControlledEnemy != null)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.right * horizontal + transform.forward * vertical).normalized;

        // keep gravity (y velocity) while overwriting x/z
        Vector3 targetVelocity = moveDir * speed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }
}
