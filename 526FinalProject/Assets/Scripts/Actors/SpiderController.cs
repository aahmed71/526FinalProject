using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float moveRange = 5.0f;
    public float moveSpeed = 2.0f;  
    public GameObject player;
    public Vector2 bodySpawnPoint;
    public GameObject body;
    private Vector2 currentTarget;

    private Vector2 startPoint;
    private Vector2 endPoint;
    void Start()
    {
        startPoint = new Vector2(transform.position.x, transform.position.y);
        endPoint = startPoint;
        endPoint.y -= moveRange;
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

       
    }

    void killPlayer()
    {
        player.GetComponent<PlayerController>().Die("Spider");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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

    public void SpiderDeath()
    {
        Instantiate(body, bodySpawnPoint, Quaternion.identity);
    }
}
