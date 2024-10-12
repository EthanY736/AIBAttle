using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Reference to the Pause Menu Canvas

    private bool isPaused = false;

    void Start()
    {
        // Ensure the Pause Menu is hidden at the start
        pauseMenuCanvas.SetActive(false);
        
        // Ensure the cursor is locked and hidden at the start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the Esc key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Function to pause the game
    void PauseGame()
    {
        pauseMenuCanvas.SetActive(true); // Show the Pause Menu Canvas
        Time.timeScale = 0f; // Freeze the game
        isPaused = true; // Set the game to paused
        
        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Function to resume the game
    void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false); // Hide the Pause Menu Canvas
        Time.timeScale = 1f; // Resume the game
        isPaused = false; // Set the game to unpaused
        
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
