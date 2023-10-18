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
    public bool canBePossessed = true;
    protected bool isPossessed = false;
    private bool canJump=false;
    //number to control how big the possession marker is
    public float markerScale = 1;

    //components
    protected Rigidbody2D rb;
    void Start()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
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
    }

    //function that's called by player if they possess the player, can be overridden
    public virtual void Move(float horizontal)
    {
        //movement
        Vector2 movement = new Vector2(horizontal, 0.0f);
        //move rigidbody
        rb.position += movement * (speed * Time.deltaTime);
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
}
