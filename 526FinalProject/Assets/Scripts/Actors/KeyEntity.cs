using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : EntityController
{
    public int keyIndex;
    
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Key");
    }
    
    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        GameManager.Instance.CalculateUnPosessionCount("Key");
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
