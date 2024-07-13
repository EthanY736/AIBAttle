using UnityEngine;

public class CharacterController1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal"); // Left and Right movement
        float moveVertical = Input.GetAxis("Vertical"); // Forward and Backward movement

        Vector3 forwardMovement = transform.forward * moveVertical;
        Vector3 rightMovement = transform.right * moveHorizontal;
        Vector3 movement = (forwardMovement + rightMovement).normalized * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply vertical rotation to the camera
        transform.Rotate(Vector3.up * mouseX); // Apply horizontal rotation to the character
    }
}
