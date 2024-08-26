using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBase
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private PlayerAbilities playerAbilities;
    public LayerMask wallLayer;  // Assign the wall layer in the Inspector
    public float rayLength = 0.1f; // Length of the raycast for collision detection



    protected override void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        playerAbilities = GetComponent<PlayerAbilities>(); // This line assigns the PlayerAbilities component

        base.Start();
        
    }

    protected override void Update()
    {
        ProcessMovementInputs();
        ProcessAbilityInputs();
    }

    private void ProcessAbilityInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerAbilities.ActivateAbility(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerAbilities.ActivateAbility(1);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessMovementInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        // Check if there's an obstacle in the direction of movement
        if (CanMove(moveDirection))
        {
            rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the player if a wall is detected
        }
    }

    bool CanMove(Vector2 direction)
    {
        // Perform a raycast in the direction of movement
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, rayLength, wallLayer);

        // Return true if there's no wall in the way, false if there is
        return hit.collider == null;
    }
}
