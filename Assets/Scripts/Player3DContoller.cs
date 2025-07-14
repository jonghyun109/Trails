using UnityEngine;

public class Player3DController : WalkerBase
{
    public float jumpForce = 40f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.8f;

    private Rigidbody rb;
    private Animator animator;

    public bool isGrounded;

    void Start()
    {
        if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Direction = new Vector3(h, 0, v);
        Walk(Direction);

        // �ȱ� �ִϸ��̼�
        bool isWalking = Direction.magnitude > 0;
        animator.SetBool("Walk", isWalking);

        // �¿� ����
        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x); // �����̸� ����, �������̸� ���
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            MoveSpeed *= 1.3f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            MoveSpeed = 10;

        // ����
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            animator.SetTrigger("Jump");
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
}
