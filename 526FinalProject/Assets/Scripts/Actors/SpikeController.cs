using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // check if the ghost was recently unpossessed by a hazard and because of it, is currently in an invincible period duration.
        if(collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().Die("Spike");
        }

        //check if the entity that we are colliding with is the same entity that the player is currently possessing.
        else if(collision.gameObject.CompareTag("Entity") && player!=null && player.GetComponent<PlayerController>().currentEntity == collision.gameObject.GetComponent<EntityController>())
        {
            player.GetComponent<PlayerController>().UnPossess();
            collision.gameObject.GetComponent<EntityController>().TakeHazardHit();
            GameObject.Find("Player").GetComponent<PlayerController>().Die("Spike");
        }

      

    }
}
