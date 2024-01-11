
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private bool isGrounded;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateAnimations();

    }
    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, 0);
        movement.Normalize();

        // Move the Rigidbody2D
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }
    
    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    void UpdateAnimations()
    {
        if (Mathf.Abs(rb.velocity.x) < 0.1f && isGrounded)
        {
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsJumping", false);
        }
        else
        {
            // Check for walking
            if (isGrounded)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);
            }
            else
            {
                // Player is in the air, consider jumping
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", true);
            }

            // Flip the sprite if moving left
            if (rb.velocity.x < 0)
            {
                FlipSprite(true);
            }
            else if (rb.velocity.x > 0) // Added condition to handle right movement
            {
                FlipSprite(false);
            }
        }
    }

    void FlipSprite(bool facingLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingLeft ? -1 : 1;
        transform.localScale = scale;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the GameObject is grounded
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Update grounded status when leaving the ground
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}