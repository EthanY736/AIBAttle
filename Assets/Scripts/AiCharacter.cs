using UnityEngine;

public class AICharacter : MonoBehaviour
{
    public float detectionRange = 10f;
    public float coneAngle = 45f; // Angle of the cone
    public int rayCount = 10; // Number of rays
    public LayerMask detectionLayer;

    void Update()
    {
        DetectObjects();
    }

    void DetectObjects()
    {
        float angleStep = coneAngle / (rayCount - 1);
        float startAngle = -coneAngle / 2;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startAngle + (i * angleStep);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange, detectionLayer))
            {
                Debug.Log("Detected: " + hit.collider.name);
                // Handle enemy/item interaction here
            }

            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }
    }
}
