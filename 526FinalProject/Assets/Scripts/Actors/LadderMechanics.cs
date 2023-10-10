using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMechanics : MonoBehaviour
{
    public float climbSpeed = 3.0f;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange)
        {
            float verticalInput = Input.GetAxis("Vertical");
            Rigidbody2D playerRB = FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>();
            Debug.Log("This is a log message." + playerRB);

            // Set the player's vertical velocity based on input.
            playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 10;
            playerInRange = false;
        }
    }
}
