using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemName;
    public bool IsCollected { get; private set; } = false;

    public void Collect()
    {
        if (!IsCollected)
        {
            IsCollected = true;
            Debug.Log($"{itemName} collected!");
            gameObject.SetActive(false);
            PaintingTracker.instance.AddPainting();
            FadeTextController.instance.TriggerText();
        }
    }
}
