using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuUI;             // The entire pause menu panel
    public Slider sensitivitySlider;      // Slider UI element
    public TMP_Text sensitivityValueText; // TMP text to show current sensitivity
    public PlayerMovement player;         // Reference to PlayerMovement script

    private bool isPaused = false;

    // Define sensitivity limits
    private const float minSensitivity = 200f;
    private const float maxSensitivity = 800f;

    void Start()
    {
        // Ensure time is running
        Time.timeScale = 1f;

        // Configure slider limits
        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;

        // Clamp player's starting sensitivity to be within range
        player.mouseSensitivity = Mathf.Clamp(player.mouseSensitivity, minSensitivity, maxSensitivity);

        // Initialize slider
        sensitivitySlider.value = player.mouseSensitivity;
        UpdateSensitivityText(player.mouseSensitivity);

        // Listen for slider changes
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        // Hide menu at start
        if (menuUI != null)
            menuUI.SetActive(false);
        else
            Debug.LogError("PauseMenu: menuUI is NOT assigned!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Toggle pause menu with SPACE key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // --- Update Player Sensitivity ---
    void OnSensitivityChanged(float newValue)
    {
        player.mouseSensitivity = Mathf.Clamp(newValue, minSensitivity, maxSensitivity);
        UpdateSensitivityText(player.mouseSensitivity);
    }

    void UpdateSensitivityText(float value)
    {
        sensitivityValueText.text = $"Sensitivity: {value:F0}";
    }

    // --- Pause Menu State ---
    void Pause()
    {
        isPaused = true;
        menuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        menuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}
