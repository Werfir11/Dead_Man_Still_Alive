using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotesttrigger : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.clip = audioClip;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            audioSource.Play();
        }
    }
}
