using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public class CannonBallDetection : MonoBehaviour
{
    public CannonBallBehaviour cannonball = null;
    private void OnTriggerEnter2D(Collider2D col)
    {
        //checks if trigger collides with an entity
        if (col.CompareTag("Entity"))
        {
            //checks if entity is a cannonball
            CannonBallBehaviour cb = col.gameObject.GetComponent<CannonBallBehaviour>();
            if (cb)
            {
                //Cannonball detected
                cannonball = cb;
                cannonball.canBePossessed = false;
            }
            else
            {
                if(!cannonball)
                    cannonball = null;
            }
        }
        else
        {
            if(!cannonball)
                cannonball = null;
        }
    }

    public void Fire(float firingSpeed)
    {
        if (cannonball)
        {
            cannonball.GetComponent<Rigidbody2D>().AddForce(transform.right * firingSpeed);
            cannonball.canBePossessed = true;
            cannonball = null;
        }
    }
}
