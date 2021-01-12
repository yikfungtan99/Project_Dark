using System;
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

    //Ladder Field
    public bool onLadder;
    public bool ladderBelow;
    [SerializeField] private float climbSpeed = 1.0f;
    private float initGravity;

    //Misc
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [SerializeField] private Collider2D environmentCollider;
    [SerializeField] private Collider2D platformCollider;
    [SerializeField] private Collider2D hitCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>() : null;

        initGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Input
        MovementInput();
        JumpInput();
        LadderInput();

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
        if (Input.GetKeyDown(KeyCode.W) && !onLadder)
        {
            Jump();
        }
    }

    private void LadderInput()
    {
        if (onLadder)
        {
            platformCollider.enabled = false;
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector2(rb.velocity.x, -climbSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            rb.gravityScale = 0;
            
        }
        else
        {
            rb.gravityScale = initGravity;
            platformCollider.enabled = true;
        }

        if (ladderBelow)
        {
            if (Input.GetKey(KeyCode.S))
            {
                platformCollider.enabled = false;
                rb.velocity = new Vector2(rb.velocity.x, -climbSpeed);
            }
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
