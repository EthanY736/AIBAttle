using UnityEngine;

public class XRayVision : MonoBehaviour
{
    public GameObject targetItem; // The item you want to glow through walls
    public Shader xRayShader;     // The x-ray shader
    public Shader originalShader; // The original shader of the item
    public KeyCode activationKey = KeyCode.X; // The key to activate x-ray vision
    private bool isXRayActive = false; // To track if x-ray is active

    private Renderer itemRenderer; // Reference to the renderer of the item

    void Start()
    {
        if (targetItem != null)
        {
            itemRenderer = targetItem.GetComponent<Renderer>();

            if (itemRenderer == null)
            {
                Debug.LogError("No Renderer component found on the target item!");
                return;
            }

            // Save the original shader
            originalShader = itemRenderer.material.shader;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            ToggleXRayVision();
        }
    }

    void ToggleXRayVision()
    {
        if (targetItem == null || itemRenderer == null) return;

        isXRayActive = !isXRayActive;

        if (isXRayActive)
        {
            // Set the x-ray shader
            itemRenderer.material.shader = xRayShader;
        }
        else
        {
            // Revert to the original shader
            itemRenderer.material.shader = originalShader;
        }
    }
}
