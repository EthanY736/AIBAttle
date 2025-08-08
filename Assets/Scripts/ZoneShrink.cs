using UnityEngine;


public class ZoneShrink : MonoBehaviour
{
    [Header("Shrink Settings")]
    public Vector3 shrinkRate = new Vector3(0.1f, 0f, 0.1f); // Shrink per second
    public Vector3 minimumScale = new Vector3(1f, 1f, 1f);    // Minimum allowed size
    public Vector3 pivotOffset = new Vector3(0f, 0f, 0f);     // Local offset of pivot (set manually if pivot isn't at center)


    private Vector3 lastScale;


    void Start()
    {
        lastScale = transform.localScale;
    }


    void Update()
    {
        Vector3 scaleChange = shrinkRate * Time.deltaTime;
        Vector3 newScale = transform.localScale - scaleChange;


        // Clamp the new scale
        newScale.x = Mathf.Max(newScale.x, minimumScale.x);
        newScale.y = Mathf.Max(newScale.y, minimumScale.y);
        newScale.z = Mathf.Max(newScale.z, minimumScale.z);


        // Calculate the actual scale delta
        Vector3 deltaScale = newScale - lastScale;


        // Apply position shift based on pivot offset and scale delta
        transform.position += new Vector3(
            pivotOffset.x * deltaScale.x,
            pivotOffset.y * deltaScale.y,
            pivotOffset.z * deltaScale.z
        );


        // Apply new scale
        transform.localScale = newScale;
        lastScale = newScale;
    }
}



