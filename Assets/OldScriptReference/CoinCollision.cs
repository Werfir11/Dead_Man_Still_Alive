using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    public CoinManager cm;
    [SerializeField] private ParticleSystem coinParticle;

    private ParticleSystem coinParticleInstance;

    private AudioSource audioSource;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            audioSource = other.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(audioSource.clip, other.transform.position);

            Destroy(other.gameObject);
            cm.coinCount++;

            coinParticleInstance = Instantiate(coinParticle, other.transform.position, Quaternion.Euler(0, 0, 40));
        }
    }
}
