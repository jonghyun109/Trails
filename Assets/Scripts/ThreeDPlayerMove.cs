using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDPlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;             
    public float jumpForce = 40f;               
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.8f;    
    public float acceleration = 10f;         

    private Rigidbody rb;
    private Vector3 inputDir;
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
        inputDir = new Vector3(h, 0, v).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        MoveSmooth();
        CheckGround();
    }

  
    void MoveSmooth()
    {
        Vector3 targetVelocity = inputDir * moveSpeed;
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 smoothVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * acceleration);

        rb.velocity = new Vector3(smoothVelocity.x, rb.velocity.y, smoothVelocity.z);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Y √ ±‚»≠
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (groundCheckDistance + 0.1f));
    }
}
