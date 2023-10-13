using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : MonoBehaviour
{
    // checks if puzzle piece is placed in puzzle wall
    private bool placed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Place()
    {
        placed = true;
        GetComponent<EntityController>().canBePossessed = false;
    }

    public bool CheckedPlaced()
    {
        return placed;
    }
}
