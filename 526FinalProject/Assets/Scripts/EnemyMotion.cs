using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion : MonoBehaviour

{
    public float moveSpeed = 2.0f; // Adjust the speed as needed
    public float changeInterval = 2.0f; // Time interval for changing direction
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private float timer;
    private Vector2 randomDirection;// Start is called before the first frame update
    void Start()
    {
        timer = changeInterval;
        randomDirection = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        // If it's time to change direction, choose a new random direction
        if (timer <= 0)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            timer = changeInterval;
        }

        // Calculate the new position
        Vector2 newPosition = (Vector2)transform.position + randomDirection * moveSpeed * Time.deltaTime;

        // Clamp the new position to stay within the defined bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Move the object to the clamped position
        GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }
}
