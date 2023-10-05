using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShootingComponent : MonoBehaviour
{
    //bullet firing logic
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float bulletDistance = 100f;
    public KeyCode fireKey = KeyCode.J;
    private Vector2 fireDirection = Vector2.right;
    
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        
        float horizontalVelocity = rb.velocity.x;
        if (Input.GetKeyDown(fireKey))
        {
            FireBullet();
        }
        if (horizontalVelocity < 0)
        {
            fireDirection = Vector2.left; // Set direction to left
        }
        else if(horizontalVelocity > 0)
        {
            fireDirection = Vector2.right; // Set direction to right
        }
    }
    
    //bullet firing logic
    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        brb.velocity = fireDirection * bulletSpeed;
        Destroy(bullet, bulletDistance / bulletSpeed);
    }
}
