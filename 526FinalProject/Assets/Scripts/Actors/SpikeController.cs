using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
 

    // time for which the ghost enjoys invincibility after getting unpossessed by the hazard
    public float invincibilityDuration = 1.5f;
    // current timer for the invincible period
    private float secondChanceTimer = 0.0f;
    // if invincible mode is enabled
    private int secondChanceFlag = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // check if the ghost was recently unpossessed by a hazard and because of it, is currently in an invincible period duration.
        if(collision.gameObject.CompareTag("Player") && secondChanceFlag==0)
        {
            player.GetComponent<PlayerController>().Die();
        }

        //check if the entity that we are colliding with is the same entity that the player is currently possessing.
        else if(collision.gameObject.CompareTag("Entity") && player!=null && player.GetComponent<PlayerController>().currentEntity == collision.gameObject.GetComponent<EntityController>())
        {
            player.GetComponent<PlayerController>().UnPossess();

            //enable second chance invincibility
            secondChanceFlag=1;
        }

      

    }
}
