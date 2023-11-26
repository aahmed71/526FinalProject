using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player") || (col.transform.CompareTag("Entity") && col.gameObject.GetComponent<EntityController>().isPossessed))
        {
            if(GameManager.Instance)
                GameManager.Instance.GameWin();
        }
    }
}


