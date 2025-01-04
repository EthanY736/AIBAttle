using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ObjectInteractionManager : MonoBehaviour
{
    public static ObjectInteractionManager instance;
    // Array to hold the interactable objects
    public GameObject[] interactableObjects;
    //public string sceneName = "YourSceneName"; // The name of the scene to load

    public GameObject EndText;
    public GameObject EndPointDoor;
    // Counter for interacted objects
    private int interactedCount = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EndText.SetActive(false);
        EndPointDoor.SetActive(false);
    }
    public void ObjectInteracted()
    {
        // Increment the interacted count
        interactedCount++;

        // Check if all objects have been interacted with
        if (interactedCount >= interactableObjects.Length)
        {
            // Perform the task (e.g., activate the task object)
            //SceneManager.LoadScene(sceneName);
            EndText.SetActive(true);
            EndPointDoor.SetActive(true);
        }
    }
    public void SetDoorActive()
    {
        EndPointDoor.SetActive(true);
    }
}

