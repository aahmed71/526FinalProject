using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallBehaviour : EntityController
{
    
    public float rollingSpeed = 10.0f;
    public float maxAccelerationForce = 40.0f;   // Maximum acceleration force
    public float decelerationRate = 45.0f;        // Rate of deceleration

    private bool isAccelerating = false;
    private float currentAccelerationForce = 0f;

    public GameObject player;
    private PlayerController playerRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Move(float horizontalInput)
    {
        // This behavior is not applicable to the cannonball objects that are fired from the cannon. ie. this is meant for cannonballs that do not come from cannon
        if(gameObject.tag!="Fired")
        {
            Rigidbody2D cannonBallRigidBody = gameObject.GetComponent<Rigidbody2D>();
            //cannonBallRigidBody.AddTorque(10.0f);

            // Check if the horizontal movement keys are pressed
            if (Input.GetKey("a") || Input.GetKey("d"))
            {
                Debug.Log("Yes");
                isAccelerating = true;
            }

            // Check if the horizontal movement keys are released
            //if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            else
            {
                Debug.Log("no");
                isAccelerating = false;
            }

            // Calculate acceleration or deceleration
            if (isAccelerating)
            {
                // Increase the acceleration force up to the maximum value
                currentAccelerationForce += maxAccelerationForce * Time.deltaTime;
                currentAccelerationForce = Mathf.Min(currentAccelerationForce, maxAccelerationForce);
            }
            else
            {
                // Decelerate gradually
                currentAccelerationForce -= decelerationRate * Time.deltaTime;
                currentAccelerationForce = Mathf.Max(currentAccelerationForce, 0.5f);
            }

            
            // Calculate the acceleration vector based on input and acceleration value
            Vector2 accelerationVector = new Vector2(horizontalInput, 0).normalized * currentAccelerationForce;

            // Apply the acceleration as a force to the Rigidbody2D
            cannonBallRigidBody.AddForce(accelerationVector);
        }
    }

    public override void Jump()
    {
        // nothing;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't want the cannonballs that are striking the non-destroyable wall to get collected and clutter in a place.
        // Hence, whenever a fired cannonball comes in contact with a platform object, cannonball gets destroyed. 
        // (Note: not applicable to cannonballs not fired from cannon)
        
        if(gameObject.tag=="Fired" && (collision.gameObject.name=="Platform")){
            gameObject.tag="Entity";
        }
    }
}
