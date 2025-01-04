using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
public Transform player; // assign the player in the Inspector

void LateUpdate()
{
    Vector3 newPosition = player.position;
    newPosition.y = transform.position.y; // keep the camera height fixed
    transform.position = newPosition;
}

}
