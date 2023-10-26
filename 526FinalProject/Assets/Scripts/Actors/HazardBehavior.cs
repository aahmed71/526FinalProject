using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] pathArray;
    private int destinationIdx;
    private int totalPoints;
    public float speed = 5.0f;
    private Vector3 directionToDestination;
    public GameObject player;

    // time for which the ghost enjoys invincibility after getting unpossessed by the hazard
    public float invincibilityDuration = 1.5f;
    // current timer for the invincible period
    private float secondChanceTimer = 0.0f;
    // if invincible mode is enabled
    private int secondChanceFlag = 0;

    void Start()
    {
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
        // check if second chance invincibility is enabled, if yes then maintain the timer
        if(secondChanceFlag==1 && secondChanceTimer<invincibilityDuration)
        {
            secondChanceTimer+=Time.deltaTime;
        }
        else
        {
            secondChanceFlag=0;
            secondChanceTimer=0.0f;
        }

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
     
    void killPlayer()
    {
        
        player.GetComponent<PlayerController>().Die("hz");


    }
    /*  void OnCollisionEnter2D(Collision2D collision)
      {
          // check if the ghost was recently unpossessed by a hazard and because of it, is currently in an invincible period duration.
          if(collision.gameObject.CompareTag("Player") && secondChanceFlag==0)
          {
              killPlayer();
          }

          //check if the entity that we are colliding with is the same entity that the player is currently possessing.
          else if(collision.gameObject.CompareTag("Entity") && player!=null && player.GetComponent<PlayerController>().currentEntity == collision.gameObject.GetComponent<EntityController>())
          {
              player.GetComponent<PlayerController>().UnPossess();

              //enable second chance invincibility
              secondChanceFlag=1;
          }

      }*/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && secondChanceFlag == 0)
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

            }
        }
    }

    }
