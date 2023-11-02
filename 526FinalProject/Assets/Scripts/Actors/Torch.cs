using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject fire;
    void Start()
    {
        fire.SetActive(false);
    }

    //turns on flame if lit by lighter
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Flame"))
        {
            fire.SetActive(true);
        }
    }
}
