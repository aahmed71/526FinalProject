using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterMechanics : EntityController
{
    private bool playerInRange = false;
    public float fireRange = 5f;

    protected override void Ability()
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
}

