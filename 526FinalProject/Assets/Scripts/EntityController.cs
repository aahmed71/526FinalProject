using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 50.0f;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private bool allowVerticalMovement = false;
    
    private Rigidbody2D rb;
    void Start()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    //function for when the player first initially possesses entity
    public virtual void OnPossess(PlayerController player)
    {
        Debug.Log("Possessed entity!");
    }

    //function that's called by player if they possess the player, can be overridden
    public virtual void Move(float horizontal, float vertical, bool jumpPressed)
    {
        //movement
        Vector2 movement = new Vector2(horizontal, 0.0f);
        
        //use vertical movement if checked
        if (allowVerticalMovement)
        {
            movement.y = vertical;
        }
        
        //move rigidbody
        rb.velocity = movement * speed;
        
        //jump
        if (jumpPressed)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce));
        }
    }
}
