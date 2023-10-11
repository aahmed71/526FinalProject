using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMechanics : MonoBehaviour
{
    private float climbSpeed = 10.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float originalGravity;

    private void Update()
    {
        if (playerInRange)
        {
            float verticalInput = Input.GetAxis("Vertical");
            if (playerRB)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed);
            }
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
