using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 2.0f;
    public float moveDuration = 2.0f;
    
    private Vector3 initialPosition;
    private bool movingRight = true;
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MoveBackAndForth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveBackAndForth()
    {
        float startTime = Time.time;

        while (Time.time - startTime < 2 * moveDuration) // 2 times the move duration
        {
            float distanceMoved = (Time.time - startTime) * moveSpeed;
            if (movingRight)
            {
                transform.position = initialPosition + Vector3.right * distanceMoved;
            }
            else
            {
                transform.position = initialPosition + Vector3.right * (2 * moveDuration * moveSpeed - distanceMoved);
            }

            if (distanceMoved >= 2 * moveDuration * moveSpeed) // 2 times the total distance
            {
                movingRight = !movingRight;
                startTime = Time.time;
            }

            yield return null; // Wait for the next frame
        }

        // Disappear by disabling the GameObject
        gameObject.SetActive(false);
    }
}
