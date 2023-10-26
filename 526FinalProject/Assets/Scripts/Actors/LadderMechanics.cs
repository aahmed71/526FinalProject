using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class LadderMechanics : EntityController
{
    private float climbSpeed = 10.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    public bool isVertical = true;
    private Vector2 initialSize;

    //analytics

    public override void Update()
    {
        base.Update();
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

    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Ladder");
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        GameManager.Instance.CalculateUnPosessionCount("Ladder");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    void SetVertical()
    {
        //Vector3 offset = GetBottomRightCorner();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        float offsetX = collider.size.x * 0.5f;
        float offsetY = collider.size.y * 0.5f;
        Vector3 point = new Vector3(0, - offsetX + offsetY, 0);
        transform.position += point;
        transform.Rotate(0, 0, -90);
        isVertical = true;
    }

    void SetHorizontal()
    {
        //Vector3 offset = GetBottomRightCorner();

        transform.Rotate(0, 0, 90);

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        float offsetX = collider.size.x * 0.5f;
        float offsetY = collider.size.y * 0.5f;
        Vector3 point = new Vector3(0, offsetX - offsetY, 0);
        transform.position += point;

        isVertical = false;
    }

    protected override void Ability()
    {
        if (isVertical)
        {
            SetHorizontal();
        }
        else
        {
            SetVertical();
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
            
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            playerRB = null;
            playerInRange = false;
        }
    }
}
