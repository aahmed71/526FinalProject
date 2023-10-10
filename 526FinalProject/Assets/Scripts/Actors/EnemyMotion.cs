using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour

{
    public float moveSpeed = 2.0f; 
    public float changeInterval = 2.0f; 
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private float timer;
    private Vector2 randomDirection;
    void Start()
    {
        timer = changeInterval;
        randomDirection = Random.insideUnitCircle.normalized;
    }

   
    void Update()
    {
        timer -= Time.deltaTime;
 
        if (timer <= 0)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            timer = changeInterval;
        }

        
        Vector2 newPosition = (Vector2)transform.position + randomDirection * moveSpeed * Time.deltaTime;

       
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Destroy the enemy when hit by a bullet
        }
    }
}
