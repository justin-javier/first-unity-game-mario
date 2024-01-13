using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float wallCheckDistance = 0.2f; // Adjust the distance based on your scene

    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    private bool isGrounded;
    private bool isTouchingWall;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsBottomTouchingGround();
        isTouchingWall = IsTouchingWall();

        Move();
        UpdateAnimations();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0);
        movement.Normalize();

        // Check if touching a wall and moving towards it
        if (isTouchingWall && Mathf.Sign(horizontalInput) == Mathf.Sign(transform.localScale.x))
        {
            // Prevent movement into the wall
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            // Move using velocity
            rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
            if (isGrounded)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsJumping", false);
            }
            else
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsJumping", true);
            }

            // Flip the sprite if moving left
            if (rb.velocity.x < 0)
            {
                FlipSprite(true);
            }
            else if (rb.velocity.x > 0)
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

    bool IsBottomTouchingGround()
    {
        Vector2 bottomCenter = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        float raycastLength = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(bottomCenter, Vector2.down, raycastLength, groundLayerMask);
        return hit.collider != null && hit.collider.CompareTag("Ground");
    }

    bool IsTouchingWall()
    {
        Vector2 frontCenter = new Vector2(boxCollider.bounds.center.x + (boxCollider.bounds.size.x / 2f), boxCollider.bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(frontCenter, Vector2.right * transform.localScale.x, wallCheckDistance, groundLayerMask);
        return hit.collider != null && !hit.collider.CompareTag("Ground");
    }
}
