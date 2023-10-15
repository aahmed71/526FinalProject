using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Entity"))
        {
            CannonBallBehaviour cannonBall = col.gameObject.GetComponent<CannonBallBehaviour>();
            if (cannonBall)
            {
                if (cannonBall.isFired)
                {
                    cannonBall.isFired = false;
                    Destroy(gameObject);
                }
                    
            }
        }
    }
}
