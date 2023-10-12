using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrigger : EntityController
{
    
    private float timeElapsedSinceLastLaunch = 0.0f;
    public float timeBetweenLaunches = 3.0f;
    public GameObject cannonLauncher;
    public GameObject spawnPoint;
    public GameObject cannonBallPrefab;
    public float cannonBallInitialSpeed = 10.0f;
    private GameObject[] fired_balls;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Call the Launch() function after every timeElapsedSinceLastLaunch seconds
        timeElapsedSinceLastLaunch += Time.deltaTime;
        if(timeElapsedSinceLastLaunch>=timeBetweenLaunches){
            Launch();
            timeElapsedSinceLastLaunch = 0.0f;
        }
    }

    void Launch()
    {
        // Create a cannonball prefab instance at the spawn point attached to the cannon launcher rectangle.
        GameObject cannonBall = Instantiate(cannonBallPrefab,spawnPoint.transform.position,Quaternion.identity);

        // Differentiate this type of cannonball instance from non-fired cannonball instances by adding a new tag - Fired.
        cannonBall.tag = "Fired";

        Rigidbody2D cannonBallRigidBody = cannonBall.GetComponent<Rigidbody2D>();

        // Launch Angle of cannonball is same as that of the cannon ball launcher rectangle
        float launchAngle = cannonLauncher.transform.eulerAngles.z;

        // Calculate the initial velocity in x and y components
        float radianAngle = launchAngle * Mathf.Deg2Rad;
        Vector2 initialVelocity = new Vector2(cannonBallInitialSpeed * Mathf.Cos(radianAngle), cannonBallInitialSpeed * Mathf.Sin(radianAngle));

        // Apply the initial velocity to the Rigidbody
        cannonBallRigidBody.velocity = initialVelocity;
    }

    
}
