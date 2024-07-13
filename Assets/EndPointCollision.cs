using UnityEngine;

public class EndPointCollision : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject endPoint;

    private void Start()
    {
        if (spawnPoint != null)
        {
            // Optionally move the player to the spawn point at the start
            // Assuming you have a player GameObject tagged as "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Assuming the player has a tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
                Debug.Log("Completed");
        }
        else
        {
            Debug.Log("Broken");
        }
    }
}
