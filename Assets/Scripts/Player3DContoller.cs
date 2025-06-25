using UnityEngine;

public class Player3DController : WalkerBase
{
    public float jumpForce = 40f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.8f;

    private Rigidbody rb;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Direction = new Vector3(h, 0, v);
        Walk(Direction);

        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    protected override void Move(Vector3 movement)
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (groundCheckDistance + 0.1f));
    }
}
