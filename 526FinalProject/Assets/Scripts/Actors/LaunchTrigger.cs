using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrigger : EntityController
{
    
    //private int isLaunchedFlag = 0;
    public GameObject cannonBarrel;
    
    
    public GameObject spawnPoint;
    public GameObject cannonBallPrefab;
    public float cannonBallInitialSpeed = 10.0f;
    //private GameObject[] fired_balls;
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
        /*// Create a cannonball prefab instance at the spawn point attached to the cannon launcher rectangle.
        GameObject cannonBall = Instantiate(cannonBallPrefab,spawnPoint.transform.position,Quaternion.identity);

        // Differentiate this type of cannonball instance from non-fired cannonball instances by adding a new tag - Fired.
        cannonBall.tag = "Fired";

        
        playerRef.UnPossess();
        playerRef.Possess(cannonBall.GetComponent<EntityController>());

        Rigidbody2D cannonBallRigidBody = cannonBall.GetComponent<Rigidbody2D>();

        // Launch Angle of cannonball is same as that of the cannon ball launcher rectangle
        float launchAngle = cannonLauncher.transform.eulerAngles.z;

        // Calculate the initial velocity in x and y components
        float radianAngle = launchAngle * Mathf.Deg2Rad;
        Vector2 initialVelocity = new Vector2(cannonBallInitialSpeed * Mathf.Cos(radianAngle), cannonBallInitialSpeed * Mathf.Sin(radianAngle));

        // Apply the initial velocity to the Rigidbody
        cannonBallRigidBody.velocity = initialVelocity;*/

    }
    
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
    }
    
    public override void Move(float horizontalInput)
    {
        
        float rotationAmount = -1 * horizontalInput * speed * Time.deltaTime;
        Debug.Log("Rotation: " + rotationAmount);
        
        if (!(cannonBarrel.transform.rotation.z <= -0.2f && rotationAmount < 0.0f ||
              cannonBarrel.transform.rotation.z >= 0.7f && rotationAmount > 0.0f))
        {
            cannonBarrel.transform.Rotate(0, 0, rotationAmount);
        }

        if(cannonBarrel.transform.rotation.z is > -0.2f and < 0.7f)
        {
            
        }
        
        // Cannon's angle is controllable by 'a' and 'd' keys
        //float newRotationZComponent = transform.eulerAngles.z + horizontalInput*Time.deltaTime*cannonRotateSpeed;
        //cannonBarrel.transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, newRotationZComponent);
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
    }

    public override void Jump()
    {
        // nothing
    }

    
}
