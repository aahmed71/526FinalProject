using UnityEngine;
using System.Collections;

public class BombMechanics : EntityController
{

    public float explosionRadius = 5.0f;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float explosionSpeed = 0.3f;
    public void Explode()
    {
        //start the bomb explosion
       StartCoroutine(DestroyBombWithDelay());
    }


    private IEnumerator DestroyBombWithDelay()
    {
        _animator.SetTrigger("StartExplosion");
        yield return new WaitForSeconds(2.5f);

        StartCoroutine(LerpScale(explosionRadius, explosionSpeed));
        yield return new WaitForSeconds(explosionSpeed);
        //detects all colliders within the explosion radius
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in objectsInRange)

        {
            if (col.CompareTag("Hazard") || col.CompareTag("Door") || col.CompareTag("Barrier"))
            {
                Destroy(col.gameObject);
            }
        }

        //unpossess player if possessed
        if (playerRef)
        {
            playerRef.UnPossess();
        }
        
        canBePossessed = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
    IEnumerator LerpScale(float end, float lerpSpeed)
    {
        float timer = 0;
        while(timer < 1)
        {
            float scale = Mathf.Lerp(0.0f, end, timer);
            _explosion.transform.localScale = new Vector3(scale, scale, 1.0f);
            

            timer = timer + Time.deltaTime / lerpSpeed;
            yield return new WaitForEndOfFrame();
        }

        _explosion.transform.localScale = new Vector3(end, end, 1.0f);
        
    }


    public override void OnPossess(PlayerController player)
    {
        base.OnPossess(player);
        GameManager.Instance.CalculatePosessionCount("Bomb");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = false;
    }

    public override void OnUnPossess(PlayerController player)
    {
        base.OnUnPossess(player);
        GameManager.Instance.CalculateUnPosessionCount("Bomb");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Collider2D coll = GetComponent<Collider2D>();
        coll.isTrigger = true;


    }
  
}


