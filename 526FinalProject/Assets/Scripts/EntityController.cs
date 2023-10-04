using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 50.0f;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private bool allowVerticalMovement = false;
    [SerializeField] private float maxHealth;
    [NonSerialized] PlayerController playerRef;
    private float currentHealth;
    public bool canBePossessed = true;
    
    private Rigidbody2D rb;
    void Start()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
        
        //set health
        currentHealth = maxHealth;
    }

    //function for when the player first initially possesses entity
    public virtual void OnPossess(PlayerController player)
    {
        playerRef = player;
    }

    //function that's called by player if they possess the player, can be overridden
    public virtual void Move(float horizontal, float vertical)
    {
        //movement
        Vector2 movement = new Vector2(horizontal, 0.0f);
        
        //use vertical movement if checked
        if (allowVerticalMovement)
        {
            movement.y = vertical;
        }
        
        //move rigidbody
        rb.position += movement * speed * Time.deltaTime;
    }

    public virtual void Jump()
    {
        rb.AddForce(new Vector2(0.0f, jumpForce));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerRef.UnPossess();
            canBePossessed = false;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        Color tempColor = GetComponent<SpriteRenderer>().color;
        tempColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = tempColor;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
