using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public CharacterController char_ctrl;
    public Rigidbody rb;
    public float speed;
    public float jump_force;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vmove += (Vector3.up * jump_force);
        }

        char_ctrl.Move(Vmove * Time.deltaTime);
    }

    
}
