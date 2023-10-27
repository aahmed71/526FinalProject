using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterMechanics : EntityController
{
    private float fireRange = 5f;
    public GameObject flameObject;
    
    protected override void Ability()
    {

        BombMechanics[] bombs = FindObjectsOfType<BombMechanics>();


        if (flameObject != null)
        {
            foreach (BombMechanics bomb in bombs)
        {
            //Check for bomb only within certain distance from the lighter
            float distanceToBomb = Vector3.Distance(transform.position, bomb.transform.position);
            if (distanceToBomb <= fireRange)
            {
                bomb.Explode();
                flameObject.SetActive(true); // Make the flame visible when a bomb explodes

            }
        }
 

        }

    }


    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Lighter");
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        GameManager.Instance.CalculateUnPosessionCount("Lighter");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("CheckPoint"))
        {
            Debug.Log("Enemy entered the trigger!");
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerController.ReachedCheckpoint();
            }
            
        }
    }
}

