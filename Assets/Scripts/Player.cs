using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private PlayerControls playerControls;
    private Rigidbody2D rigidBody;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float gravityScaleAtStart;

    [SerializeField] private float speed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float climbSpeed;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask climbLadder;


    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerControls.Land.Jump.performed += _ => Jump();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        gravityScaleAtStart = rigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Land.Sprint.activeControl != null) {
            if (IsGrounded()) {
                float movementInput = playerControls.Land.Move.ReadValue<float>();

                Vector2 currentPosition = transform.position;
                currentPosition.x += movementInput * sprintMultiplier * Time.deltaTime;
                transform.position = currentPosition;
            }
        }
        Run();
        FlipSprite();
        ClimbLadder();
    }

    private void Run()
    {
        float movementInput = playerControls.Land.Move.ReadValue<float>();

        Vector2 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;

        if (movementInput == 0)
        {
            animator.SetBool("Running", false);
        } else
        {
            animator.SetBool("Running", true);
        }
    }

    private void ClimbLadder()
    {
        if (collider.IsTouchingLayers(climbLadder))
        {
            float climbInput = playerControls.Land.ClimbLadder.ReadValue<float>();
            Vector2 rigidBodyVelocity = new Vector2(rigidBody.velocity.x, climbInput * climbSpeed);
            rigidBody.velocity = rigidBodyVelocity;
            rigidBody.gravityScale = 0f;

            if (climbInput == 0)
            {
                animator.SetBool("Climbing", false);
            }
            else
            {
                animator.SetBool("Climbing", true);
            }
        } 
        else
        {
            animator.SetBool("Climbing", false);
            rigidBody.gravityScale = gravityScaleAtStart;
        }
    }

    private void FlipSprite()
    {
        float movementInput = playerControls.Land.Move.ReadValue<float>();
        if (movementInput > 0)
        {
            spriteRenderer.flipX = false;
        } 
        else if (movementInput < 0)
        {
            spriteRenderer.flipX = true;
        }

    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed);
            rigidBody.velocity = jumpVelocityToAdd;
        }
    }

    private bool IsGrounded()
    {
        return collider.IsTouchingLayers(ground);
    }
}
