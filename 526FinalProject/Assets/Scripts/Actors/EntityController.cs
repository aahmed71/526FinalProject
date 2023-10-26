using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] protected float jumpForce = 50.0f;
    [SerializeField] protected float speed = 10.0f;
    [SerializeField] protected KeyCode utilityButton = KeyCode.F;
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
    public Rigidbody2D rb;
    void Start()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
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

    protected void CheckJump(Transform collider)
    {
        if (collider.transform.position.y < transform.position.y && !canJump)
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
            rb.AddForce(new Vector2(0.0f, jumpForce));
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
