using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Movement : MonoBehaviour
{
    public CharacterController char_ctrl;
    public Rigidbody rb;
    public float speed;
    public float jump_height;
    public float gravityValue;

    Vector3 playerVelocity;
    void Start ()
    {
    }

    void Update ()
    {

        Vector3 Vmove = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.D))
        {
           Vmove += (Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vmove += (Vector3.left * speed);
        }

        if (Input.GetKeyDown(KeyCode.Space) & char_ctrl.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jump_height * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        char_ctrl.Move((Vmove + (playerVelocity.y * Vector3.up))* Time.deltaTime);
    }

    
}
