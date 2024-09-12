using System;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class NinjaMovement : MonoBehaviour
{
    [Header("Horizontal Attributes")]
    public float minXSpeed = 3f;
    public float maxXSpeed = 10f;           // Maximum horizontal speed
    public float rightAccelerationTime = 3f;
    public float leftAccelerationTime = 3f;
    public float breakingTime = 0.5f;       //how long it takes to break by pushing into the other direction
    public float slowdownTime = 2f;         //upon letting go of a direction, how long it takes to stop
    [Tooltip("1 = same as ground, below 1 = faster than ground, above 1 = slower than ground")]
    public float airControl = 2f;           // 1 = same as ground, below 1 = faster than ground, above 1 = slower than ground

    [Header("Vertical Attributes")]
    public float jumpSpeed = 8f;            // Additional force applied while holding jump
    public float maxJumpTime = 0.17f;       // Maximum time the jump can be held
    public float gravityScale = 4f;         // Custom gravity scale to adjust falling speed
    public LayerMask groundLayer;           // Layer representing the ground
    public Transform groundCheck;           // Position of the ground check object
    public float groundCheckRadius = 0.2f;  // Radius for ground check overlap

    [Header("Fast Fall Attributes")]
    public float fastFallSpeed = 20f;
    public float maxYSpeed = 20f;           // Maximum vertical speed

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTimeCounter;
    private bool hasTriggeredFastFall;      // Flag to check if fast-fall has been triggered

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        CheckIfGrounded();
        Movement();
        Jump();
        Fastfall();
        ClampSpeed();
    }


    void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            hasTriggeredFastFall = false;
        }
    }

    void Movement()
    {
        bool rightPressed = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool leftPressed = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool noDirectionPressed = !rightPressed && !leftPressed;

        bool isMovingRight = rb.velocity.x >= 0.01;
        bool isMovingLeft = rb.velocity.x <= -0.01;
        bool isIdle = !isMovingRight && !isMovingLeft;

        float currentXSpeed = rb.velocity.x;

        if (isMovingRight)
        {
            if (noDirectionPressed)
            {
                ApplyNextLerpT(currentXSpeed, maxXSpeed, slowdownTime, false);
            }
            else if (rightPressed)
            {
                ApplyNextLerpT(currentXSpeed, maxXSpeed, rightAccelerationTime, true);
            }
            else if (leftPressed)
            {
                ApplyNextLerpT(currentXSpeed, maxXSpeed, breakingTime, false);
            }
        }
        if (isMovingLeft)
        {
            if (noDirectionPressed)
            {
                ApplyNextLerpT(currentXSpeed, -maxXSpeed, slowdownTime, false);
            }
            else if (leftPressed)
            {
                ApplyNextLerpT(currentXSpeed, -maxXSpeed, rightAccelerationTime, true);
            }
            else if (rightPressed)
            {
                ApplyNextLerpT(currentXSpeed, -maxXSpeed, breakingTime, false);
            }
        }
        if (isIdle)
        {
            if (noDirectionPressed)
            {
                return;
            }
            if (rightPressed)
            {
                rb.velocity = new Vector2(minXSpeed, rb.velocity.y);
            }
            if (leftPressed)
            {
                rb.velocity = new Vector2(-minXSpeed, rb.velocity.y);
            }
        }

        void ApplyNextLerpT(float current, float max, float totalTime, bool positive)
        {
            totalTime = isGrounded ? totalTime : totalTime * airControl;
            float alreadyLerped = current / max;
            float percentageTimePassed = Time.deltaTime / totalTime;
            if (positive)
            {
                float t = alreadyLerped + percentageTimePassed;
                float nextSpeed = Mathf.Lerp(0, max, t);
                rb.velocity = new Vector2(nextSpeed, rb.velocity.y);
            }
            else
            {
                float t = alreadyLerped - percentageTimePassed;
                float nextSpeed = Mathf.Lerp(0, max, t);
                rb.velocity = new Vector2(nextSpeed, rb.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    void Fastfall()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (rb.velocity.y < 0 && !hasTriggeredFastFall)
            {
                rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
                hasTriggeredFastFall = true;
            }
        }
    }

    void ClampSpeed()
    {
        Vector2 clamp = rb.velocity;

        if (Mathf.Abs(clamp.x) > maxXSpeed)
        {
            clamp.x = Mathf.Sign(clamp.x) * maxXSpeed;
        }

        if (Mathf.Abs(clamp.y) > maxYSpeed)
        {
            clamp.y = Mathf.Sign(clamp.y) * maxYSpeed;
        }

        rb.velocity = clamp;
    }

    // Optional: visualize the ground check sphere in the editor
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
