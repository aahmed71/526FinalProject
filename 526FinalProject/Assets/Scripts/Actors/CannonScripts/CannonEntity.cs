using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEntity : EntityController
{
    
    //private int isLaunchedFlag = 0;
    public GameObject cannonBarrel;
    
    [SerializeField] private float firingSpeed = 600.0f;
    [SerializeField] private GameObject cannonBallPrefab;
    [SerializeField] private Transform firingLocation;
    [SerializeField] private ParticleSystem particles;

    void Launch()
    {
        AudioManager.instance.Play("Cannon");
        particles.Play();
        // Create a cannonball prefab instance at the spawn point attached to the cannon launcher rectangle.
        GameObject cannonBall = Instantiate(cannonBallPrefab,firingLocation.position,Quaternion.identity);

        // Differentiate this type of cannonball instance from non-fired cannonball instances.
        CannonBallBehaviour cbb = cannonBall.GetComponent<CannonBallBehaviour>();
        cbb.OnFired(cannonBarrel.transform.right * firingSpeed);
        
        //have player possess the cannonball after firing
        playerRef.UnPossess();
        playerRef.Possess(cbb);
    }

    protected override void Ability()
    {
        Launch();
    }

    public override void Move(float horizontalInput)
    {
        //Instead of moving the entity, for the cannon it rotates the barrel
        float rotationAmount = -1 * horizontalInput * speed * Time.deltaTime;
        
        if (!(cannonBarrel.transform.rotation.z <= -0.2f && rotationAmount < 0.0f ||
              cannonBarrel.transform.rotation.z >= 0.7f && rotationAmount > 0.0f))
        {
            cannonBarrel.transform.Rotate(0, 0, rotationAmount);
        }
    }

    public override void Jump()
    {
        // entity cannot jump
    }
    
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Cannon");
    }
    
    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
       
    }

    
}
