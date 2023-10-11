using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public int lockIndex;

    private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Entity"))
        {
            KeyEntity keyComp = col.gameObject.GetComponent<KeyEntity>();
            if (keyComp)
            {
                if (keyComp.keyIndex == lockIndex)
                {
                    Destroy(door.gameObject);
                }
            }
        }
    }
}
