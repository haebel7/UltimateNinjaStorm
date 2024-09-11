using System;
using System.Xml.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class NinjaMovement : MonoBehaviour
{
    public static int xd = 100;

    [Header("Movement Attributes")]
    public float minXSpeed = 3f;            // Movement speed
    public float acceleration = 5f;         // Movement speed
    public float maxXSpeed = 10f;           // Maximum horizontal speed
    public float maxYSpeed = 20f;           // Maximum vertical speed

    [Header("Jump Attributes")]
    public float jumpSpeed = 8f;            // Additional force applied while holding jump
    public float maxJumpTime = 0.17f;       // Maximum time the jump can be held
    public float gravityScale = 4f;         // Custom gravity scale to adjust falling speed
    public LayerMask groundLayer;           // Layer representing the ground
    public Transform groundCheck;           // Position of the ground check object
    public float groundCheckRadius = 0.2f;  // Radius for ground check overlap

    [Header("Fast Fall Attributes")]
    public float fastFallSpeed = 20f;

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
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        if (left && right)
        {
            return;
        }
        if (right)
        {
            if (rb.velocity.x <= 0.1 && rb.velocity.x >= -0.1)
            {
                rb.velocity = new Vector2(minXSpeed, rb.velocity.y);
            }

            else
            {
                Debug.Log("Are you doing something?");
                rb.AddForce(new Vector2(acceleration * Time.deltaTime, 0));
            }
        }
        if (left)
        {
            if (rb.velocity.x <= 0.1 && rb.velocity.x >= -0.1)
            {
                rb.velocity = new Vector2(-minXSpeed, rb.velocity.y);
            }

            else
            {
                rb.AddForce(new Vector2(-acceleration * Time.deltaTime, 0));
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
