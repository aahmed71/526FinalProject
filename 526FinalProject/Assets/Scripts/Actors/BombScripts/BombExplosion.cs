using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        //explodes certain objects
        if (col.CompareTag("Hazard") || col.CompareTag("Door") || col.CompareTag("Barrier"))
        {
            Destroy(col.gameObject);
        }
    }
}
