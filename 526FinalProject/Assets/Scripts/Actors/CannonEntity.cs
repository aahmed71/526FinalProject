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
    //public GameObject player;
    //private PlayerController playerRef;

    // Start is called before the first frame update
    void Start()
    {
        //playerRef = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*// Call the Launch() function after every timeElapsedSinceLastLaunch seconds
        if(isLaunchedFlag==0 && Input.GetKey(utilityButton) && playerRef.currentEntity.gameObject.name == "CannonBallLauncher"){
            Launch();
            isLaunchedFlag=1;
        }*/
    }

    void Launch()
    {
        // Create a cannonball prefab instance at the spawn point attached to the cannon launcher rectangle.
        GameObject cannonBall = Instantiate(cannonBallPrefab,firingLocation.position,Quaternion.identity);

        // Differentiate this type of cannonball instance from non-fired cannonball instances.
        CannonBallBehaviour cbb = cannonBall.GetComponent<CannonBallBehaviour>();
        cbb.OnFired(cannonBarrel.transform.right * firingSpeed);
        
        playerRef.UnPossess();
        playerRef.Possess(cbb);

        //Rigidbody2D cannonBallRigidBody = cannonBall.GetComponent<Rigidbody2D>();

        // Launch Angle of cannonball is same as that of the cannon ball launcher rectangle
        /*float launchAngle = cannonLauncher.transform.eulerAngles.z;

        // Calculate the initial velocity in x and y components
        float radianAngle = launchAngle * Mathf.Deg2Rad;
        Vector2 initialVelocity = new Vector2(cannonBallInitialSpeed * Mathf.Cos(radianAngle), cannonBallInitialSpeed * Mathf.Sin(radianAngle));

        // Apply the initial velocity to the Rigidbody
        cannonBallRigidBody.velocity = initialVelocity;*/

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

    
}
