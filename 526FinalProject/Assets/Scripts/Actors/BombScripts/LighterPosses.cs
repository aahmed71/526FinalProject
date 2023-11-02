using System.Collections;
using UnityEngine;

public class LighterMechanics : EntityController
{
    private float fireRange = 5f;
    public GameObject flameObject;
    private float flameDuration = 2f; // Duration for which the flame stays on

    // This method is called to use the lighter's ability
    protected override void Ability()
    {
        if (flameObject != null)
        {
            if (flameObject.activeSelf)
            {
                flameObject.SetActive(false); // Deactivate the flame object
            }
            else
            {
                flameObject.SetActive(true); // Activate the flame object
            }
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (flameObject.activeSelf)
        {
            IgniteNearbyBombs(); // Ignite nearby bombs
        }
    }


    // Method to check for and ignite nearby bombs
    private void IgniteNearbyBombs()
    {
        BombMechanics[] bombs = FindObjectsOfType<BombMechanics>();
        foreach (BombMechanics bomb in bombs)
        {
            float distanceToBomb = Vector3.Distance(transform.position, bomb.transform.position);
            if (distanceToBomb <= fireRange)
            {
                bomb.Explode(); // Call the Explode method on the bomb
            }
        }
    }

    // Called when the lighter is possessed by a player
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Lighter");
    }

    // Called when the lighter is unpossessed by a player
    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        GameManager.Instance.CalculateUnPosessionCount("Lighter");
    }

    // Unity's OnTriggerEnter2D method, called when another collider enters this object's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CheckPoint") && isPossessed)
        {
            Debug.Log("Enemy entered the trigger!");
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerController.ReachedCheckpoint(other.transform.position);
            }
        }
    }
}
