using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : EntityController
{
    
    public Transform[] pathArray;
    private int destinationIdx;
    private int totalPoints;
    public float speed = 5.0f;
    private Vector3 directionToDestination;
    public GameObject player;
    [SerializeField] private float detectionRadius = 10.0f;
    private GameObject chaseModeUI;
    private bool dead = false;
    
    // parameters for dead hazard possesion
    [SerializeField] private float mass = 100.0f;
    [SerializeField] private float gravScale = 10f;
    void Start()
    {
        OnStart();
        chaseModeUI = transform.GetChild(1).gameObject;
        totalPoints = pathArray.Length;
        destinationIdx = Mathf.Min(1,totalPoints-1);
        if(totalPoints>0)
        {
            transform.position = pathArray[0].position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the GameObject and the player
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // hazard is dead
        if (dead)
        {
            return;
        }
        // If the player is within the detection radius
        if (distance <= detectionRadius)
        {
            chaseModeUI.SetActive(true);

            // Move the GameObject towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            chaseModeUI.SetActive(false);
            
            directionToDestination = pathArray[destinationIdx].position-transform.position;
            if(directionToDestination.magnitude > 1)
            {
                directionToDestination.Normalize();
                transform.position += directionToDestination * speed * Time.deltaTime;
            }
            else
            {
                destinationIdx = (destinationIdx+1)%totalPoints;
            }            
        }

    }
     
    void killPlayer()
    {
        
        player.GetComponent<PlayerController>().Die("Devil");


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // hazard is dead
        if (dead)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            killPlayer();

        }
        EntityController entity = collision.gameObject.GetComponent<EntityController>();
        if (entity != null)
        {

            // If the player is currently possessing this entity, unpossess it.
            if (player.GetComponent<PlayerController>().currentEntity == entity)
            {
                player.GetComponent<PlayerController>().UnPossess();
                entity.TakeHazardHit();
                killPlayer();
            }
        }
    }

    public void Death()
    {
        dead = true;
        // Change hazard into entity
        chaseModeUI.SetActive(false);
        gameObject.tag = "DeadHazard";
        gameObject.layer = 6;
        rb.isKinematic = false;
        rb.mass = mass;
        rb.gravityScale = gravScale;
    }
}
