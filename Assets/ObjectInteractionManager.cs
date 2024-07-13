using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteractionManager : MonoBehaviour
{
    // Array to hold the interactable objects
    public GameObject[] interactableObjects;

    // Counter for interacted objects
    private int interactedCount = 0;

    public void ObjectInteracted()
    {
        // Increment the interacted count
        interactedCount++;

        // Check if all objects have been interacted with
        if (interactedCount >= interactableObjects.Length)
        {
            // Perform the task (e.g., activate the task object)
            Debug.Log("All objects interacted with! Task completed!");

        }
    }
}

