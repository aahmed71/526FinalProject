using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMechanics : EntityController
{
    private float climbSpeed = 10.0f;
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float originalGravity;
    public bool isVertical = true;
    private Vector2 initialSize;

    protected override void Start()
    {
        base.Start();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider)
        {
            initialSize = collider.size;
        }
    }

    public override void Update()
    {
        base.Update();
        if (playerInRange && isVertical && playerRB)
        {
            float verticalInput = Input.GetAxis("Vertical");
            playerRB.velocity = new Vector2(playerRB.velocity.x, verticalInput * climbSpeed * Time.deltaTime);
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
        rb.isKinematic = false;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    void SetVertical()
    {
        Vector3 offset = new Vector3(initialSize.x / 2, -initialSize.y / 2, 0);

        // Translate, rotate, then translate back.
        transform.Translate(offset);
        transform.Rotate(0, 0, -90);
        transform.Translate(-offset);

        isVertical = true;
    }

    void SetHorizontal()
    {
        Vector3 offset = new Vector3(initialSize.x / 2, -initialSize.y / 2, 0);

        // Translate, rotate, then translate back.
        transform.Translate(offset);
        transform.Rotate(0, 0, 90);
        transform.Translate(-offset);

        isVertical = false;
    }

    public override void Move(float horizontal)
    {
        base.Move(horizontal);
        if (Input.GetKeyDown(utilityButton))
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
