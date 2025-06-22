using UnityEngine;

public class PlayerController : WalkerBase
{

    public float jumpForce = 5f;
    public Rigidbody2D rb;

    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveSpeed = 10f;

    }

    void Update()
    {
        Debug.Log("MoveSpeed: " + MoveSpeed);
        float moveInput = Input.GetAxisRaw("Horizontal");
        Debug.Log("Move Input: " + moveInput);
        Direction = new Vector2(moveInput, 0);
        Move(Direction);
        Walk(Direction);
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            MoveSpeed = 10f * 1.3f;
        }
        else
        {
            MoveSpeed = 10f;
        }

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        
    }


    protected override void Move(Vector2 movement)
    {
        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
