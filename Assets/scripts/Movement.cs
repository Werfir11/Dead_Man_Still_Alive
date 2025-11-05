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
    public float gravity_value;

    int air_jumps_left;

    Vector3 playerVelocity;
    void Start ()
    {
        air_jumps_left = 1;
    }

    void Update ()
    {

        Vector3 Vmove = new Vector3(0, 0, 0);

        //left
        if (Input.GetKey(KeyCode.D))
        {
           Vmove += (Vector3.right * speed);
        }

        //right
        if (Input.GetKey(KeyCode.A))
        {
            Vmove += (Vector3.left * speed);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && (char_ctrl.isGrounded || air_jumps_left != 0))
        {
            playerVelocity.y = Mathf.Sqrt(jump_height * -2.0f * gravity_value);
            if (!char_ctrl.isGrounded) { air_jumps_left--; Debug.Log("test1"); }
        }

        if (char_ctrl.isGrounded) { air_jumps_left = 1; Debug.Log("dotykam ziemi"); }

        playerVelocity.y += gravity_value * Time.deltaTime; //gravity

        char_ctrl.Move((Vmove + (playerVelocity.y * Vector3.up))* Time.deltaTime); //move character
    }

    
}
