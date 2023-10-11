using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMechanics : EntityController
{
    private float climbSpeed = 10.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float originalGravity;
    public bool standing = true;

    public override void Update()
    {
        base.Update();
        if (playerInRange)
        {
            if (standing)
            {
                float verticalInput = Input.GetAxis("Vertical");
                if (playerRB)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed);
                }
            }

        }

    }

    public override void Move(float horizontal)
    {
        base.Move(horizontal);
        if (Input.GetKeyDown(utilityButton))
        {
            standing = !standing;
            transform.Rotate(0, 90, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB)
            {
                originalGravity = playerRB.gravityScale;
                playerRB.gravityScale = 0;
            }
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRB)
            {
                playerRB.gravityScale = originalGravity;
                playerRB = null;
            }
            playerInRange = false;
        }
    }
}
