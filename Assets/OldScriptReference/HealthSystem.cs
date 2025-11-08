using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;
    private bool isInvincible;
    public float invincibilityDuration;
    public MovementR move;
    public GameObject UI;
    public GameObject deathScreen;
    private int damageTriggerCount = 0;

    [Header("References")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip deathClip;

    public AudioSource audioSource;

    private void Awake()
    {
        playerSprite.color = Color.white;
        currentHealth = maxHealth;
    }
    public void Damage()
    {
        if (isInvincible || currentHealth == 0) return;

        currentHealth -= 1;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0) {
            audioSource.clip = damageClip;
            audioSource.Play();
            StartCoroutine(Invincibility());
        } 
        else
        {
            audioSource.clip = deathClip;
            audioSource.Play();
            Death();
        }
    }
    public void Death()
    {
        move.movementEnabled = false;
        move.rb.linearVelocity = Vector2.zero;
        playerSprite.color = Color.black;

        UI.SetActive(false);
        deathScreen.SetActive(true);
    }
    public IEnumerator Invincibility()
    {
        isInvincible = true;
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(invincibilityDuration - 1.5f);
        playerSprite.color = Color.yellow;
        yield return new WaitForSeconds(invincibilityDuration - .5f);
        playerSprite.color = Color.white;
        isInvincible = false;

        if (damageTriggerCount > 0) {
            Damage();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("damageTrigger"))
        {
            damageTriggerCount++;
            Damage();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("damageTrigger"))
        {
            damageTriggerCount--;
            Mathf.Abs(damageTriggerCount);
        }
    }
}
