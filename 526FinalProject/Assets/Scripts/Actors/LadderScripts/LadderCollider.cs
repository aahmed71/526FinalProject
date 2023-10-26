using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCollider : MonoBehaviour
{
    private float climbSpeed = 20.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float defaultGravityScale = 10.0f;

    void Update()
    {
        if (playerInRange && playerRB)
        {
            float verticalInput = Input.GetAxis("Vertical");

            //makes sure we only apply velocity to possessed entities or the player
            if (playerRB.CompareTag("Entity") && !playerRB.gameObject.GetComponent<EntityController>().isPossessed)
            {
                return;
            }
            playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Entity"))
        {
            playerRB = other.GetComponent<Rigidbody2D>();

            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Entity"))
        {

            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
            playerRB = null;
            playerInRange = false;
        }
    }
}
