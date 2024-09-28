using UnityEngine;

public class NearestPaintingMaterialSwitcher : MonoBehaviour
{
    public Material material1;  // First material
    public Material material2;  // Second material
    public float detectionRadius = 20f;  // How far to look for paintings
    public string paintingTag = "Painting";  // Tag used to find painting objects (case-sensitive)
    public Color gizmoColor = Color.yellow;  // Color of the wireframe in the Scene view

    private Transform playerTransform;  // Reference to the player's position
    private Renderer nearestPaintingRenderer;  // Renderer of the nearest painting
    private bool isMaterial1Active = true;  // Track which material is active
    private GameObject nearestPainting;  // The nearest painting object

    void Start()
    {
        // Get the player's transform at the start
        playerTransform = this.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Find the nearest painting to the player
            FindNearestPainting();
            if (nearestPainting != null && nearestPaintingRenderer != null)
            {
                // Toggle the material of the nearest painting
                ToggleMaterial(nearestPaintingRenderer);
            }
            else
            {
                Debug.Log("No Painting Found or no Renderer attached.");
            }
        }
    }

    void FindNearestPainting()
    {
        // Find all objects with the tag "Painting"
        GameObject[] paintings = GameObject.FindGameObjectsWithTag(paintingTag);
        float nearestDistance = Mathf.Infinity;  // Start with a large distance
        nearestPainting = null;
        nearestPaintingRenderer = null;  // Reset renderer

        // Iterate over all found paintings to determine the closest one
        foreach (GameObject painting in paintings)
        {
            float distance = Vector3.Distance(playerTransform.position, painting.transform.position);
            if (distance < nearestDistance && distance <= detectionRadius)  // Check within detection radius
            {
                nearestDistance = distance;
                nearestPainting = painting;
                nearestPaintingRenderer = painting.GetComponent<Renderer>();  // Get the Renderer component

                // Ensure the painting has a Renderer
                if (nearestPaintingRenderer == null)
                {
                    Debug.LogWarning("Nearest painting found, but it has no Renderer component.");
                    nearestPainting = null;  // Reset if no renderer
                }
            }
        }

        if (nearestPainting != null)
        {
            Debug.Log("Nearest painting found: " + nearestPainting.name + " at distance " + nearestDistance);
        }
        else
        {
            Debug.Log("No paintings found or within detection radius.");
        }
    }

    void ToggleMaterial(Renderer paintingRenderer)
    {
        if (isMaterial1Active)
        {
            paintingRenderer.material = material2;
            Debug.Log("Material 2 Loaded");
        }
        else
        {
            paintingRenderer.material = material1;
            Debug.Log("Material 1 Loaded");
        }
        isMaterial1Active = !isMaterial1Active;  // Switch between materials
    }

    // Draws the detection radius as a wireframe in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
