using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    protected float jumpForce = 4000.0f;
    protected float speed = 30.0f;
    protected KeyCode utilityButton = KeyCode.F;
    [NonSerialized] protected PlayerController playerRef;
    [NonSerialized] public bool canBePossessed = true;
    [NonSerialized]public bool isPossessed = false;
    public bool canJump=false;
    //number to control how big the possession marker is
    public float markerScale = 1;
    private int hazardHits = 0;
    private int maxHazardHits = 1;
    private Vector3 gameStartPosition;
    [SerializeField] protected float maxVertVel = 50f;
    public float groundCheck = 5;

    //components
    protected Rigidbody2D rb;
    
    protected virtual void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        gameStartPosition = transform.localPosition;
        
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EntityController: No RigidBody2D found on the GameObject!");
        }
        rb.mass = 100;
        rb.gravityScale = 10;
    }

    public virtual void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (isPossessed && Input.GetKeyDown(utilityButton))
        {
            Ability();
        }
    }

    protected void CheckJump(Transform col)
    {
        RaycastHit2D[] results = new RaycastHit2D[2];
        int num = Physics2D.Raycast(transform.position, Vector2.down, new ContactFilter2D().NoFilter(), results, 5.5f);
        
        if (!canJump && num > 1 && !canJump)
        {
            canJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        CheckJump(col.transform);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rb.velocity.y is < 1 and > -1)
        {
            CheckJump(collision.transform);
        }
    }

    //function for when the player first initially possesses entity
    public virtual void OnPossess(PlayerController player)
    {
        playerRef = player;
        isPossessed = true;
        GameManager.Instance.controlDisplay.SetText(ControlDisplay.ControlType.Possession, "Unpossess");
    }

    //function for when the player is unpossesses entity
    public virtual void OnUnPossess(PlayerController player)
    {
        isPossessed = false;
        rb.velocity = Vector2.zero;
        GameManager.Instance.controlDisplay.SetText(ControlDisplay.ControlType.Possession, "Possess");
    }

    //function that's called by player if they possess the player, can be overridden
    public virtual void Move(float horizontal)
    {
        //move rigidbody
        Vector2 vel = rb.velocity;
        vel.x = horizontal * speed;
        rb.velocity = vel;
    }

    protected virtual void Ability()
    {
        //ability code would go here
    }

    public virtual void Jump()
    {
        if (canJump)
        {
            AudioManager.instance.Play("Jump");
            // prevent player from applying excess force when flying around in dead hazard
            // Apply jump force
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);

            float vertVel =  Mathf.Min(rb.velocity.y, maxVertVel);
            rb.velocity = new Vector2(rb.velocity.x, vertVel);
            if (!gameObject.CompareTag("DeadHazard"))
            {
                canJump = false;
            }
        }
    }

    private void OnDeath()
    {
        Color tempColor = GetComponent<SpriteRenderer>().color;
        tempColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = tempColor;
        playerRef.UnPossess();
        canBePossessed = false;
    }
    
    public virtual void TakeHazardHit()
    {
        transform.position = gameStartPosition;
        hazardHits++;
        if (hazardHits >= maxHazardHits)
        {
            //BecomeUnpossessable();

        }
    }
}
