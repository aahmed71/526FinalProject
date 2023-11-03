using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    
    public Vector2 startPoint = new Vector2(-28, 5.6f); 
    public Vector2 endPoint = new Vector2(-28, -7.5f);  
    public float moveSpeed = 2.0f;  
    public GameObject player;
    public float invincibilityDuration = 1.5f;
    private float secondChanceTimer = 0.0f;
    private int secondChanceFlag = 0;             
    private Vector2 currentTarget;
    void Start()
    {
        currentTarget = endPoint;
    }

    void Update()
    {
       
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentTarget) < 0.01f)
        {
            if (currentTarget == endPoint)
            {
                currentTarget = startPoint;
            }
            else
            {
                currentTarget = endPoint;
            }
        }

        if(secondChanceFlag==1 && secondChanceTimer<invincibilityDuration)
        {
            secondChanceTimer+=Time.deltaTime;
        }
        else
        {
            secondChanceFlag=0;
            secondChanceTimer=0.0f;
        }
       
    }

    void killPlayer()
    {
        player.GetComponent<PlayerController>().Die("spider");
    }

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
