using UnityEngine;
using System.Collections;

public class BombMechanics : EntityController
{
    private bool playerInRange = false;

    public float explosionRadius = 5.0f; 
    private Rigidbody2D playerRB;
    private float originalGravity;

    [SerializeField] private Animator _animator;

    public void Explode()
    {
        _animator.SetTrigger("StartExplosion");
        //start the bomb explosion
       StartCoroutine(DestroyBombWithDelay());
    }


    private IEnumerator DestroyBombWithDelay()
    {
        
        yield return new WaitForSeconds(5f);
        //detects all colliders within the explosion radius
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Hazard") || col.CompareTag("Door"))
            {
                Destroy(col.gameObject);
            }
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // Implement the OnPossess and OnUnPossess 
    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);

        Debug.Log("Possed called for"+gameObject.name);
        isPossessed = true;

        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        Debug.Log("OnUnPossess called for " + gameObject.name);

        isPossessed = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;


    }
  
}


