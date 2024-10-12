using UnityEngine;

public class MaterialToggle : MonoBehaviour
{
    public Material material1; // First material to toggle to
    public Material material2; // Second material to toggle to
    public KeyCode toggleKey = KeyCode.T; // The key to press for toggling

    private Renderer objectRenderer; // The renderer component of the object
    private bool isUsingMaterial1 = true; // Tracks which material is currently applied

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer component found on this object!");
            return;
        }

        // Set the initial material to material1 if it's assigned
        if (material1 != null)
        {
            objectRenderer.material = material1;
        }
    }

    void Update()
    {
        // Check if the toggle key is pressed
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleMaterial();
        }
    }

    void ToggleMaterial()
    {
        if (objectRenderer == null) return;

        // Toggle between material1 and material2
        if (isUsingMaterial1 && material2 != null)
        {
            objectRenderer.material = material2;
            isUsingMaterial1 = false;
        }
        else if (!isUsingMaterial1 && material1 != null)
        {
            objectRenderer.material = material1;
            isUsingMaterial1 = true;
        }
    }
}
