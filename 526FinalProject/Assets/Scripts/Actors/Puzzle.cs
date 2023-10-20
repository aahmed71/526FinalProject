using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Puzzle : MonoBehaviour
{
    // Colliders that checks for puzzle pieces
    private BoxCollider2D boxColl;
    
    //# of puzzle pieces needed
    [SerializeField] private int pieces;
    [SerializeField] private GameObject keyPrefab;


    private bool locked = true;

    [SerializeField] private GameObject wall;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Puzzle complete
        if (pieces == 0 && locked)
        {

            locked = false;
            // Debug.Log("You won!");
            // if (GameManager.Instance)
            // {
            //     GameManager.Instance.GameWin();
            // }
            keyPrefab.SetActive(true);
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // check if object inside is a puzzle block
        if (other.gameObject.CompareTag("PuzzleBlock"))
        {
            // make sure puzzle piece is completely inside
            if (boxColl.bounds.Contains(other.bounds.max) && boxColl.bounds.Contains(other.bounds.min))
            {
                // Check if its already placed
                if (!other.gameObject.GetComponent<PuzzleBlock>().CheckedPlaced())
                {
                    if (player.GetComponent<PlayerController>().IsPossessing())
                    {
                        player.GetComponent<PlayerController>().UnPossess();
                        // player.GetComponent<Rigidbody2D>().position = other.transform.position + new Vector3(0, 1, 0);
                        // player.transform.position = other.transform.position + new Vector3(0, 3, 0);
                        float finishTime = Time.time;
                        GameManager.Instance.BlockUnPossessTime(finishTime);
                        GameManager.Instance.CalculateUnPosessionCount("PuzzleBlock");
                    }
                    other.gameObject.GetComponent<PuzzleBlock>().Place();
                    Debug.Log("One more puzzle piece in!");
                    pieces--;
                }

            }
        }
    }
}
