using UnityEngine;

public class Painting : MonoBehaviour
{
    public bool isFound = false;  // Flag to track if the painting has been found
    public static Painting instance;
    public void Awake()
    {
        instance = this;
    }
    public void SetBoolTrue()
    {
        isFound = true;
    }
}
