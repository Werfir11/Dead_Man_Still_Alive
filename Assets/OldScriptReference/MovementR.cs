using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class MovementR : MonoBehaviour
{
    [Header("Movement Settings")]

    public float maxSpeed;
    public float speedMultiplier;
    public float jumpForce;
    public int extraJumpCount;
    public float crouchSpeedMultiplier;
    public float crouchSlideSpeed;
    public int crouchSlideCount;
    public float currSpeed;
    public float wallJumpForce;
    public float wallJumpColldown;
    public float wallSlideSpeed;

    [Header("Controls")]

    public bool movementEnabled = true;
    public bool jumpEnabled = true;
    public bool crouchEnabled = true;
    public bool crouchSlideEnabled = true;
    public bool dashEnabled = true;
    public bool wallJumpEnabled = true;

    [Header("References")]

    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Animator animator;

    [Header("Dash Settings")]
    public float dashCooldown;
    public float dashSpeed;
    public float lastDashTime;
    public Vector2 dashDirection;

    [Header("Other")]

    public float inputHorizontal;
    private bool isFacingRight = true;
    private bool isDashing;
    public bool isCrouching = false;
    public Vector2 velocity;
    public float lastWallJumpTime;
    public float time;

    [SerializeField] private ParticleSystem dashParticle;
    private ParticleSystem dashParticleInstance;


    private void Start()
    {
        lastWallJumpTime = -wallJumpColldown;
        lastDashTime = -dashCooldown;
    }
    private void Update()
    {
        if (!movementEnabled) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        handleJump();
        handleCrouch();
        handleDash();
        handleFlip();
        handleDash();
        velocity.x = rb.linearVelocity.x;
        velocity.y = rb.linearVelocity.y;
        time = Time.time;

        animator.SetFloat("XMovement", currSpeed);
        animator.SetFloat("YMovement", velocity.y);
        animator.SetBool("IsGrounded", isGrounded());
        animator.SetBool("IsWallSliding", isWallSliding());
    }

    private void FixedUpdate()
    {
        if (!movementEnabled) return;

        handleWallSlide();
        handleMovement();
    }

    private void handleMovement()
    {
        if (isCrouching)
        {
            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed*crouchSpeedMultiplier)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + inputHorizontal * crouchSpeedMultiplier, rb.linearVelocity.y);
            }
        }
        else 
        {
            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x + inputHorizontal * speedMultiplier, rb.linearVelocity.y);
            }
        }
        currSpeed = Mathf.Abs(rb.linearVelocity.x);
    }
    private void handleJump()
    {
        if (!jumpEnabled) return;

        if (isGrounded() && extraJumpCount == 0)
        {
            extraJumpCount = 1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (isWallSliding() && wallJumpEnabled && Time.time >= lastWallJumpTime+wallJumpColldown)
            {
                int direction = isFacingRight ? -1 : 1;
                rb.linearVelocity = new Vector2(direction * wallJumpForce, jumpForce);
                flip();
                lastWallJumpTime = Time.time;
            }
            else if (extraJumpCount > 0 && !isWallSliding())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce*0.7f);
                extraJumpCount--;
            }
        }
    }
    private void handleCrouch()
    {
        if (!crouchEnabled || isWallSliding() || !isGrounded()) return;
        if (Input.GetKeyDown(KeyCode.S) && !isCrouching)
        {
            isCrouching = true;
            playerCollider.size = new Vector2(1f, 1.8f);
            playerCollider.offset = new Vector2(0f, .9f);
            if (Mathf.Abs(rb.linearVelocity.x) > 10f && crouchSlideEnabled && crouchSlideCount > 0)
            {
                if (inputHorizontal == -1f)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x - crouchSlideSpeed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x + crouchSlideSpeed, rb.linearVelocity.y);
                }
                crouchSlideCount--; 
            }
        }else if(isCrouching && Input.GetKeyUp(KeyCode.S) && !isCeilinged())
        {
            isCrouching = false;
            playerCollider.offset = new Vector2(0f, 1.3f);
            playerCollider.size = new Vector2(1f, 2.6f);
        }
        if (isGrounded() && crouchSlideCount == 0 && Mathf.Abs(rb.linearVelocity.x) < 20f) 
        {
            crouchSlideCount = 1;
        }
    }
    private void handleDash()
    {
        if (!dashEnabled) return;
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Dash")) && !isDashing)
        {
            if (Time.time >= lastDashTime + dashCooldown) 
            {
                dashDirection = isFacingRight ? Vector2.right : Vector2.left;
                if (dashDirection == Vector2.right) 
                {
                    rb.linearVelocity = new Vector2(dashSpeed, rb.linearVelocity.y);
                } else
                {
                    rb.linearVelocity = new Vector2(-dashSpeed, rb.linearVelocity.y);
                }
                lastDashTime = Time.time;
                dashParticleInstance = Instantiate(dashParticle, playerTransform.position, Quaternion.Euler(270, 0, 0));
                dashParticleInstance.transform.parent = playerTransform;
            }
        }
    }
    private void handleWallSlide() 
    {
        if (!wallJumpEnabled) return;

        if (isWallSliding() && !isGrounded() && rb.linearVelocity.y < 0)
        {
            //rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y + wallSlideSpeed, -10, 100));
            rb.gravityScale = 2.4f;
        }
        else
        {
            rb.gravityScale = 4;
        }
    }
    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = playerTransform.localScale;
        scale.x *= -1;
        playerTransform.localScale = scale;
    }
    private void handleFlip()
    {
        if (inputHorizontal > 0 && !isFacingRight)
        {
            flip();
        }
        else if (inputHorizontal < 0 && isFacingRight)
        {
            flip();
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, .5f, groundLayer);
    }
    private bool isCeilinged()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, .5f, groundLayer);
    }
    private bool isWallSliding()
    {
        return Physics2D.OverlapCircle(wallCheck.position, .5f, wallLayer);
    }
    private bool isCrouched()
    {
        return isCrouching;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("geyser"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 35f);
        }
    }
}