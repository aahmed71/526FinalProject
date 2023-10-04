using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damage = 5;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Entity"))
        {
            col.gameObject.GetComponent<EntityController>().TakeDamage(damage);
        }
    }
}
