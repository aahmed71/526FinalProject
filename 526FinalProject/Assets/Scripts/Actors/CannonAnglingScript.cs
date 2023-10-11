using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnglingScript : EntityController
{
    // cannonAnglingSpeed => dictates how quick you can angle the cannon in some unit time of pressing and holding the 'a' and 'd' keys.
    public float cannonAnglingSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Move(float horizontalInput)
    {
        // Cannon's angle is controllable by 'a' and 'd' keys
        float newRotationZComponent = transform.eulerAngles.z + horizontalInput*Time.deltaTime*cannonAnglingSpeed;
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, newRotationZComponent);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
    }

    public override void Jump()
    {
        // nothing
    }
}
