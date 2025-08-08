using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    // Set this in the Inspector to the name or build index of your game level
    [SerializeField] public string gameSceneName = "GameScene";


    // Called when the Play button is clicked
    public void OnPlayButtonClicked()
    {
        Debug.Log("Loading game scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }


    // Called when the Exit button is clicked
    public void OnExitButtonClicked()
    {
        Debug.Log("Exiting game...");
        Application.Quit();


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
#endif
    }
}



