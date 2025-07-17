using System.Collections;
using Photon.Pun;
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
        rb = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            rb.freezeRotation = true;
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        if (!photonView.IsMine || isDead) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Direction = new Vector3(h, 0, v);
        Walk(Direction);

        // 걷기 애니메이션
        animator.SetBool("Walk", Direction.magnitude > 0);

        // 좌우 반전
        if (h != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            MoveSpeed *= 1.3f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            MoveSpeed = 10f;

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

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        if (photonView.IsMine && animator != null && !isDead)
        {
            animator.SetTrigger("Hit");
        }
    }

    protected override IEnumerator HandleDeath()
    {
        if (photonView.IsMine && animator != null)
        {
            animator.SetTrigger("Die");
        }

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        var anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = false;

        yield return base.HandleDeath();
    }
}
