using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endscript : MonoBehaviour
{
    public MovementR move;
    public GameObject UI;
    public GameObject endScreen;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            move.movementEnabled = false;
            move.rb.linearVelocity = Vector2.zero;

            UI.SetActive(false);
            endScreen.SetActive(true);
        }
    }
}
