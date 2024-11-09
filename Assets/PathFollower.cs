using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints;       // Array to hold the waypoints
    public float speed = 5f;            // Speed of the movement
    public float reachThreshold = 0.1f; // Threshold to determine if the waypoint is reached

    private int currentWaypointIndex = 0;
    private float initialY;             // Variable to store the starting Y position

    void Start()
    {
        transform.Rotate(0, 0, 180);
        initialY = transform.position.y; // Store the initial Y position
    }

    void Update()
    {
        // Check if there are waypoints defined
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint, keeping the initial Y position
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, initialY, targetWaypoint.position.z); // Keep Y constant
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the GameObject is close enough to the waypoint to consider it reached
        Vector3 direction = targetWaypoint.position - transform.position;
        if (direction.magnitude <= reachThreshold)
        {
            // Rotate 90 degrees around the Y-axis
            transform.Rotate(0, 0, -90);

            // Move to the next waypoint, looping if needed
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
