using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    bool jumped;

    [Header("References")]

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;


    Vector3 playerVelocity;
    void Start ()
    {
        air_jumps_left = 1;
        jumped = false;
    }

    void Update ()
    {
        
        Vector3 Vmove = new Vector3(0, 0, 0);

        //right
        if (Input.GetKey(KeyCode.D))
        {
           //transform.Rotate(0.0f, 90.0f, 0.0f);
           Vmove += (Vector3.right * speed);
        }

        //left
        if (Input.GetKey(KeyCode.A))
        {
            //transform.Rotate(0.0f, -90.0f, 0.0f);
            Vmove += (Vector3.left * speed);
        }

        //jump

        if (isGrounded())
        {
            air_jumps_left = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded())
            {
                playerVelocity.y = Mathf.Sqrt(jump_height * -2.0f * gravity_value);
            }
            else if (air_jumps_left > 0)
            {
                playerVelocity.y = Mathf.Sqrt(jump_height * -2.0f * gravity_value);
                air_jumps_left--;
            }
        }


    /*if (Input.GetKeyDown(KeyCode.Space) && (char_ctrl.isGrounded || air_jumps_left != 0))
    {
        if (!char_ctrl.isGrounded) { air_jumps_left--; Debug.Log("test1"); }
        playerVelocity.y = Mathf.Sqrt(jump_height * -2.0f * gravity_value);
    }

    if (char_ctrl.isGrounded) { air_jumps_left = 1; Debug.Log("dotykam ziemi"); }
    */
    if (!Input.GetKeyDown(KeyCode.Space) && char_ctrl.isGrounded && playerVelocity.y < 0f) playerVelocity.y = 0f;
    else playerVelocity.y += gravity_value * Time.deltaTime; //gravity

    Debug.Log("playerVelocity.y = " + playerVelocity.y);
    Debug.Log("air_jumps_left = " + air_jumps_left);

    char_ctrl.Move((Vmove + (playerVelocity.y * Vector3.up)) * Time.deltaTime); //move character
    }

    private bool isGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, .5f, groundLayer).Length > 0;
    }

}
