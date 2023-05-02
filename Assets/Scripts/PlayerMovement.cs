using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    //[SerializeField] float rollForce = 6.0f;
    private bool doubleJump;
    private bool rolling = false;
    private int  currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    //private float delayToIdle = 0.0f;
    //private float rollDuration = 8.0f / 14.0f;
    //private float rollCurrentTime;

    private enum MovementState { idle, running, jumping, falling, attacking }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        timeSinceAttack += Time.deltaTime;

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > .1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        UpdateAnimationState();

    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !rolling)
        {
            state = MovementState.attacking;
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 3)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            anim.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }
        
        else if (dirX > 0f) // Right
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f) // Left
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else // Idle
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f) // Jump
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f) // Fall
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
