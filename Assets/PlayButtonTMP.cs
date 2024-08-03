using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayButtonTMP : MonoBehaviour
{
    public string sceneName = "YourSceneName"; // The name of the scene to load

    private void Start()
    {
        // Get the Button component attached to this GameObject
        Button button = GetComponent<Button>();

        // Add a listener to the button to call the LoadScene method when clicked
        if (button != null)
        {
            button.onClick.AddListener(LoadScene);
        }
    }

    private void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
