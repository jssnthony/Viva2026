using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 10f;
    public float acceleration = 50f;
    public float deceleration = 40f;

    [Header("Salto")]
    public float jumpForce = 15f;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.7f, 0.15f);
    public LayerMask groundLayer = 1;

    [Header("Slide")]
    public float slideDuration = 0.6f;
    public float slideSpeedMultiplier = 1.5f;
    public Vector2 standingColliderSize = new Vector2(0.8f, 1.4f);
    public Vector2 slidingColliderSize = new Vector2(0.8f, 0.4f);
    public Vector2 standingColliderOffset = new Vector2(0f, 0f);
    public Vector2 slidingColliderOffset = new Vector2(0f, -0.5f);

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isSliding;
    private float slideTimer;
    private float currentSpeedX;
    private Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalColor = sprite.color;
    }

    void Update()
    {
        CheckGround();

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
                EndSlide();
        }

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            float kx = 0f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                kx = -1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                kx = 1f;
            SetMoveInput(kx);

            if (keyboard.spaceKey.wasPressedThisFrame)
                DoJump();

            if (keyboard.cKey.wasPressedThisFrame && isGrounded)
                StartSlide();
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput.x * moveSpeed;
        if (isSliding)
            targetSpeed *= slideSpeedMultiplier;

        if (moveInput.x != 0)
        {
            currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetSpeed, acceleration * Time.fixedDeltaTime);
            sprite.flipX = moveInput.x < 0;
        }
        else
        {
            currentSpeedX = Mathf.MoveTowards(currentSpeedX, 0f, deceleration * Time.fixedDeltaTime);
        }

        Vector2 vel = rb.linearVelocity;
        vel.x = currentSpeedX;
        rb.linearVelocity = vel;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
    }

    void StartSlide()
    {
        if (isSliding) return;
        isSliding = true;
        slideTimer = slideDuration;
        boxCollider.size = slidingColliderSize;
        boxCollider.offset = slidingColliderOffset;
        sprite.color = new Color(0.6f, 0.6f, 0.6f);
    }

    void EndSlide()
    {
        isSliding = false;
        boxCollider.size = standingColliderSize;
        boxCollider.offset = standingColliderOffset;
        sprite.color = originalColor;
    }

    void RestoreColor()
    {
        sprite.color = originalColor;
    }

    public void SetMoveInput(float x)
    {
        moveInput = new Vector2(x, 0f);
    }

    public void DoJump()
    {
        if (isSliding) return;
        if (!isGrounded) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        sprite.color = Color.cyan;
        Invoke("RestoreColor", 0.1f);
    }

    public void DoSlide()
    {
        if (isGrounded)
            StartSlide();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jumpPressed = true;
    }

    public void OnSlide(InputValue value)
    {
        if (value.isPressed && isGrounded)
            StartSlide();
    }

    public bool IsSliding()
    {
        return isSliding;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
