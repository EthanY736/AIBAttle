using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Reference to the interaction manager
    public ObjectInteractionManager interactionManager;

    // Flag to check if the object has already been interacted with
    private bool hasInteracted = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player interacted with the object and if it hasn't been interacted with yet
        if (other.CompareTag("Player") && !hasInteracted)
        {
            // Notify the interaction manager
            interactionManager.ObjectInteracted();

            // Set the flag to true to prevent further interactions
            hasInteracted = true;

            // Optionally, disable the collider to prevent further interactions
            GetComponent<Collider>().enabled = false;

            // Log the interaction
            Debug.Log("Painting Found!");
        }
    }
}

