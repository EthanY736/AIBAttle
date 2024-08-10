using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectInteractionManager : MonoBehaviour
{
    // Array to hold the interactable objects
    public GameObject[] interactableObjects;
     public string sceneName = "YourSceneName"; // The name of the scene to load


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
            SceneManager.LoadScene(sceneName);
        

        }
    }
}

