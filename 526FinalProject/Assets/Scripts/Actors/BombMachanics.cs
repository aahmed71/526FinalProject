using UnityEngine;
using System.Collections;

public class BombMechanics : EntityController
{

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
            if (col.CompareTag("Hazard") || col.CompareTag("Door") || col.CompareTag("Barrier"))
            {
                Destroy(col.gameObject);
            }
        }
        playerRef.UnPossess();
        canBePossessed = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;


    }
  
}


