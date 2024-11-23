using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public string[] collectibleNames = new string[5];
    private int collectedCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.0f);
            foreach (Collider hitCollider in hitColliders)
            {
                CollectibleItem item = hitCollider.GetComponent<CollectibleItem>();
                if (item != null && !item.IsCollected)
                {
                    item.Collect();
                    collectedCount++;
                    Debug.Log($"Collected {collectedCount}/{collectibleNames.Length} items.");
                    if (collectedCount == collectibleNames.Length)
                    {
                        Debug.Log("You have found all items!");
                        ObjectInteractionManager.instance.SetDoorActive(); 
                    }
                    break;
                }
            }
        }
    }
}
