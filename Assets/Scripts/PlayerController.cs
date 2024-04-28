using System;
using UnityEngine;

public class PlayerController : CharacterBase
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private PlayerAbilities playerAbilities;


    protected override void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        playerAbilities = GetComponent<PlayerAbilities>(); // This line assigns the PlayerAbilities component

        base.Start();
        
    }

    void Update()
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
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
    }
}
