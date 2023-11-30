using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //explodes certain objects
        if (col.CompareTag("Hazard") || col.CompareTag("Door") || col.CompareTag("Barrier"))
        {
            SpiderController spider = col.gameObject.GetComponent<SpiderController>();
            if (spider)
            {
                spider.SpiderDeath();
            }
            Destroy(col.gameObject);
        }
    }
}
