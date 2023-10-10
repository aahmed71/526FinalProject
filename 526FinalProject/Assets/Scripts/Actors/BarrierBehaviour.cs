using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the barrier comes in contact with a cannonball that was fired (and not just any cannonball sitting), barrier destroys.
        if(collision.gameObject.tag=="Fired"){
            Destroy(gameObject);
        }
    }
}
