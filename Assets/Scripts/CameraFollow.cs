using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    private Transform followTransform;


    public void SetTarget(Transform newTarget)
    {
        followTransform = newTarget;


        if (followTransform != null)
        {
            transform.SetParent(null); // Detach to avoid offset bugs
            transform.position = followTransform.position;
            transform.rotation = followTransform.rotation;
        }
    }


    void LateUpdate()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
            transform.rotation = followTransform.rotation;
        }
    }
}



