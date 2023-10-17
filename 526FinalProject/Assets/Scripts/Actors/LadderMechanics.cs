using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderMechanics : EntityController
{
    private float climbSpeed = 10.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float originalGravity;
    public bool isVertical = true;
    private Vector2 initialSize;

    //analytics
    public AnalyticsManager analyticsScript;

    //text mechanics
    public Text displayText;
    private TextController textController;


    public override void Update()
    {
        base.Update();
        if (playerInRange && isVertical && playerRB)
        {
            float verticalInput = Input.GetAxis("Vertical");
            playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed);
        }

    }

    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
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
        if (other.CompareTag("Player"))
        {
            playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB)
            {
                originalGravity = playerRB.gravityScale;
                playerRB.gravityScale = 0;
            }
            playerInRange = true;

            //text mechanics
            // if (textController != null)
            // {
            //     textController.ShowText();
            // }

            //analytics
            if (analyticsScript != null)
            {
                    analyticsScript.testAnalytics("Calling from spike controller");
            }

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
