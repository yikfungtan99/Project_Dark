using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    //Movement Field
    [SerializeField] private float moveSpeed;
    private float moveX;

    //JumpField
    [SerializeField] private float jumpForce;
    private float curJumpForce;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    private Rigidbody2D rb;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>() : null;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Input
        MovementInput();
        JumpInput();

        Movement();
        GroundCheck();
    }

    private void MovementInput()
    {
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;
    }

  
    private void Movement()
    {
        float xVelo = moveX * Time.deltaTime;
        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;
        float yVelo = jumpForce;
        rb.velocity = new Vector2(rb.velocity.x, yVelo);
    }

    private void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        isGrounded = collider ? true : false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
