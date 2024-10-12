using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    public CharacterController1 characterController;

    void Start()
    {
        if (sensitivitySlider != null && characterController != null)
        {
            // Set slider's min and max values
            sensitivitySlider.minValue = 0;
            sensitivitySlider.maxValue = 1;

            // Set initial value based on the character controller's sensitivity
            float initialSliderValue = Mathf.InverseLerp(100f, 300, characterController.mouseSensitivity);
            sensitivitySlider.value = initialSliderValue;

            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
        else
        {
            Debug.Log("Something Missing For SensitivitySlider");
        }
    }

    void OnSensitivityChanged(float value)
    {
        if (characterController != null)
        {
            // Map slider value to sensitivity range (100 to 200)
            float mappedSensitivity = Mathf.Lerp(100f, 300, value);
            characterController.mouseSensitivity = mappedSensitivity;
        }
    }
}
