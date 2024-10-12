using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    // Reference to the button component
    public Button quitButton;

    void Start()
    {
        // Ensure the button is assigned
        if (quitButton != null)
        {
            // Add a listener to the button to trigger the QuitGame method when clicked
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("Quit button is not assigned!");
        }
    }

    // Method to quit the game
    void QuitGame()
    {
        // If running in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build
        Application.Quit();
#endif
    }
}
