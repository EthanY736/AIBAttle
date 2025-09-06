using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 20f; // Degrees per second

    void Update()
    {
        // Rotate ONLY around the global Y axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}
