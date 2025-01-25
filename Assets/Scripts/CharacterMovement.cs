using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

    }
}
