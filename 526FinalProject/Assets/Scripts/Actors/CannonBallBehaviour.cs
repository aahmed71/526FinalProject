using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CannonBallBehaviour : EntityController
{
    [SerializeField] private float maxVelocity = 15.0f;
    public bool isFired = false;

    // Start is called before the first frame update
    void Awake()
    {
        //initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public override void Move(float horizontalInput)
    {
        //cannot move if locked into place
        if (!canBePossessed || isFired)
            return;
        
        //movement
        Vector2 movement = new Vector2(horizontalInput, 0.0f);
        //move rigidbody
        rb.AddForce(movement * speed);

        float horizv = rb.velocity.x;
        horizv = Mathf.Clamp(horizv, -maxVelocity, maxVelocity);
        rb.velocity = new Vector2(horizv, rb.velocity.y);
    }

    public void OnFired(Vector2 forceDir)
    {
        isFired = true;
        rb.AddForce(forceDir);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't want the cannonballs that are striking the non-destroyable wall to get collected and clutter in a place.
        // Hence, whenever a fired cannonball comes in contact with a platform object, cannonball gets destroyed. 
        // (Note: not applicable to cannonballs not fired from cannon)

        if (isFired && Mathf.Abs(rb.velocity.magnitude) < 5)
        {
            isFired = false;
        }
        /*if(gameObject.tag=="Fired" && (collision.gameObject.name=="Platform")){
            gameObject.tag="Entity";
        }*/
    }
}
