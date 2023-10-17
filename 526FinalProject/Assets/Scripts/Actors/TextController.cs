using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text displayText;
    public float displayDuration = 3.0f;
    private bool playerNearLadder = false;
    private float displayTimer = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearLadder)
        {
            // Show the text when the player is near the ladder
            displayText.enabled = true;

            // Count the time
            displayTimer += Time.deltaTime;

            if (displayTimer >= displayDuration)
            {
                // Hide the text after the display duration
                displayText.enabled = false;
                playerNearLadder = false;
            }
        }
    }

    public void ShowText()
    {
        playerNearLadder = true;
        displayTimer = 0.0f;
    }
}
