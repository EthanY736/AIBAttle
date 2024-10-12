using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;           // Reference to the player object (should be the parent)
    public float rotationSpeed = 5f;   // Speed at which the arrow rotates towards the target

    private Transform nearestPainting; // Reference to the nearest painting object

    void Start()
    {
        // Ensure this object (the arrow) is a child of the player
        if (player == null)
        {
            player = transform.parent; // If player is not set in the inspector, get the parent transform
        }
    }

    void Update()
    {
        FindNearestPainting();  // Find the closest painting object
        if (nearestPainting != null)
        {
            Debug.Log("Nearest Painting: " + nearestPainting.name);
            PointArrowAtPainting();  // Make the arrow point towards the painting
        }
    }

    // Find the nearest painting by comparing distances
    void FindNearestPainting()
    {
        GameObject[] paintings = GameObject.FindGameObjectsWithTag("Painting"); // Get all objects tagged "Painting"
        float shortestDistance = Mathf.Infinity;  // Initialize with an infinitely large distance
        Transform previousPainting = nearestPainting; // Track the previous nearest painting to detect changes

        // Loop through all paintings to find the closest one
        foreach (GameObject painting in paintings)
        {
            float distanceToPainting = Vector3.Distance(player.position, painting.transform.position); // Calculate distance to each painting
            if (distanceToPainting < shortestDistance)
            {
                shortestDistance = distanceToPainting;
                nearestPainting = painting.transform;  // Update the nearest painting
            }
        }

        // If the nearest painting changes, log the new target
        if (previousPainting != nearestPainting && nearestPainting != null)
        {
            Debug.Log("Arrow is locking on to: " + nearestPainting.name);  // Output the name of the nearest painting
        }
    }

    // Rotate the arrow to point towards the nearest painting on the Y-axis only
    void PointArrowAtPainting()
    {
        // Calculate the direction from the arrow (which is positioned near the player) to the painting
        Vector3 directionToPainting = nearestPainting.position - transform.position;

        // Only rotate on the Y-axis by setting the Y component of the direction to 0
        directionToPainting.y = 0; // We only want to rotate on the horizontal plane

        // Ensure that the direction has length (if the player and painting are at the same position, skip rotation)
        if (directionToPainting.magnitude > 0.1f)
        {
            // Calculate the rotation needed to face the painting
            Quaternion targetRotation = Quaternion.LookRotation(directionToPainting);

            // Smoothly rotate the arrow towards the painting only on the Y-axis
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

