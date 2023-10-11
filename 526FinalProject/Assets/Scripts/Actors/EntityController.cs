using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 50.0f;
    [SerializeField] private float speed = 10.0f;
    [NonSerialized] PlayerController playerRef;
    public bool canBePossessed = true;
    
    //components
    private HealthComponent healthComponent;
    private Rigidbody2D rb;
    void Start()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();
        
        //set health
        if (healthComponent)
        {
            healthComponent.deathEvent.AddListener(OnDeath);
        }
    }

    //function for when the player first initially possesses entity
    public virtual void OnPossess(PlayerController player)
    {
        playerRef = player;
    }

    //function that's called by player if they possess the player, can be overridden
    public virtual void Move(float horizontal)
    {
        //movement
        Vector2 movement = new Vector2(horizontal, 0.0f);

        //move rigidbody
        rb.position += movement * (speed * Time.deltaTime);
    }

    public virtual void Jump()
    {
        rb.AddForce(new Vector2(0.0f, jumpForce));
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
