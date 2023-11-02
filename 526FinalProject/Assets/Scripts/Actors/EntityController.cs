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
    private bool canJump=false;
    //number to control how big the possession marker is
    public float markerScale = 1;
    private int hazardHits = 0;
    private int maxHazardHits = 1;
    private SpriteRenderer spriteRenderer;


    //components
    protected Rigidbody2D rb;
    protected virtual void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("EntityController: No RigidBody2D found on the GameObject!");
        }
        rb.mass = 100;
        rb.gravityScale = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("EntityController: No SpriteRenderer found on the GameObject!");
        }
    }

    public virtual void Update()
    {
        if (isPossessed && Input.GetKeyDown(utilityButton))
        {
            Ability();
        }
    }

    protected void CheckJump(Transform col)
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 5 * markerScale) && !canJump && !canJump)
        {
            canJump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        CheckJump(col.transform);
    }

    //function for when the player first initially possesses entity
    public virtual void OnPossess(PlayerController player)
    {
        playerRef = player;
        isPossessed = true;
    }

    //function for when the player is unpossesses entity
    public virtual void OnUnPossess(PlayerController player)
    {
        isPossessed = false;
        rb.velocity = Vector2.zero;
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
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            canJump = false;
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
    public virtual void BecomeUnpossessable()
    {
        canBePossessed = false;
        LightenColor();

    }
    public virtual void TakeHazardHit()
    {
        hazardHits++;
        if (hazardHits >= maxHazardHits)
        {
            BecomeUnpossessable();

        }
    }
    private void LightenColor()
    {
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            float lightness = 0.5f; // Adjust as needed
            spriteRenderer.color = new Color(
                currentColor.r + lightness,
                currentColor.g + lightness,
                currentColor.b + lightness,
                currentColor.a
            );
        }
    }
    }
