using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterMechanics : EntityController
{
    private bool playerInRange = false;
    private Rigidbody2D playerRB;
    private float originalGravity;
    public float fireRange = 5f;
    //private bool isPossessed = false;

    public override void Update()
    {
        base.Update();
        if (isPossessed && Input.GetKeyDown(utilityButton))
        {
            BombMechanics[] bombs = FindObjectsOfType<BombMechanics>();


            foreach (BombMechanics bomb in bombs)
            {
                //Check for bomb only within certain distance from the lighter
                float distanceToBomb = Vector3.Distance(transform.position, bomb.transform.position);

                if (distanceToBomb <= fireRange)
                {
                    bomb.Explode();
                }

            }
        }
    
}

    /* private void UnPossessLighter()
     {
         if (playerRef != null)
         {
             playerRef.UnPossess();
         }
     }
    */
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        isPossessed = true;

        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        Debug.Log("OnUnPossess called for " + gameObject.name);

        isPossessed = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB)
            {
                originalGravity = playerRB.gravityScale;
                playerRB.gravityScale = 0;
            }
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRB)
            {
                playerRB.gravityScale = originalGravity;
                playerRB = null;
            }
            playerInRange = false;
        }
    }
}

