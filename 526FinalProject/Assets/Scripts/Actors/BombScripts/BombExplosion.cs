using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //explodes certain objects
        if (col.CompareTag("Door") || col.CompareTag("Barrier"))
        {
            Destroy(col.gameObject);
        }
        else if (col.CompareTag("Hazard"))
        {
            HazardBehavior hazard = col.gameObject.GetComponent<HazardBehavior>();
            if (hazard)
            {
                hazard.Death();
            }
            else
            {
                SpiderController spider = col.gameObject.GetComponent<SpiderController>();
                if (spider)
                {
                    spider.SpiderDeath();
                    Destroy(col.gameObject);
                }
            }

        }
    }
}
