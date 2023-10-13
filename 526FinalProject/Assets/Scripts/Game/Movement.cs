using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SquareMovement : MonoBehaviour
{
    public float moveDistance = 0.5f;
    private float direction = 1f; // 1 for right, -1 for left
    private Camera mainCamera;
    private float halfWidth;

    private void Start()
    {
        mainCamera = Camera.main;
        halfWidth = transform.localScale.x / 2;
    }

    void Update()
    {
        MoveSquare();
        CheckBoundariesAndReverse();
    }

    private void MoveSquare()
    {
        transform.position += new Vector3(moveDistance * direction, 0, 0) * Time.deltaTime;
    }

    private void CheckBoundariesAndReverse()
    {
        float leftBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x + halfWidth;
        float rightBound = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x - halfWidth;

        if (transform.position.x < leftBound || transform.position.x > rightBound)
        {
            direction *= -1; // Reverse direction
        }
    }

   /* private void TrackMovement(string direction)
    {
        Debug.Log("Sending analytics data for direction: " + direction);

        Analytics.CustomEvent("SquareMoved", new Dictionary<string, object>
        {
            { "direction", direction },
            { "x", transform.position.x },
            { "y", transform.position.y }
        });
    }*/
}
