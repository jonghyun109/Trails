using UnityEngine;

public class PlayerController : WalkerBase
{

    public float jumpForce = 5f;
    public Rigidbody2D rb;

    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField] private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Direction = new Vector2(moveInput, 0);

        Walk(Direction);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("쉬프트 누름");
            MoveSpeed = MoveSpeed * 1.3f;
        }
        else
        {
            MoveSpeed = MoveSpeed;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        
    }


    protected override void Move(Vector3 movement)
    {
        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.05f);
        }
    }
}
