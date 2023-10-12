using UnityEngine;
using System.Collections;

public class BombMechanics : MonoBehaviour
{
    public float explosionRadius = 5.0f; // Radius within which objects will be destroyed

    public void Explode()
    {
        //detects all colliders within the explosion radius
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Wall"))
            {
                // Destroy the object with the "Wall" tag
                Destroy(col.gameObject);
            }
        }

        //destroy the bomb itself after the explosion
        StartCoroutine(DestroyBombWithDelay());
    }


    private IEnumerator DestroyBombWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
       Destroy(gameObject);
    }
}
