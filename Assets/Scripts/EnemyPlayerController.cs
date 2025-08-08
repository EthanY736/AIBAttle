using UnityEngine;


public class EnemyPlayerController : MonoBehaviour
{
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public float speed = 5f; // Movement speed
    public float mouseSensitivity = 500; // Mouse sensitivity for looking
    private float rotationY = 0f; // Tracks the player's rotation on the Y-axis


    void Update()
    {
        // Only respond to input if this is the selected enemy
        if (EnemyClickTracker.ActiveControlledEnemy != gameObject)
            return;


        // Mouse input for looking left and right
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += mouseX;


        // Rotate the player around the Y-axis
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);


        // Movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // Calculate movement direction relative to the player's forward direction
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;


        // Move the player
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }




}



