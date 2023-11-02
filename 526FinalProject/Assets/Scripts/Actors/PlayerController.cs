using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this line to include the UI namespace

// using Unity.Services.Core;
// using Unity.Services.Core.Environments;
// using UnityEngine.Analytics;


public class PlayerController : MonoBehaviour
{
    //inputs
    private float horizontalInput;
    private bool jumpInputPrevious = false;
    //checkpoint code
    private int deathCount = 3;
    public GameObject startPointObject;
    public GameObject checkPointObject;
    public GameObject endPointObject;
    private Transform startPoint;
    private Transform endPoint;
    private Transform checkPoint;
    public GameObject checkpointNotif;
    public float timeToReachCheckpoint;

    public bool hasReachedCheckpoint = false;
    private bool hasReachedEndPoint = false;
    public Image heart1; 
    public Image heart2; 
    public Image heart3;

    //buttons in case we want to change them
    private KeyCode jumpButton = KeyCode.Space;
    private KeyCode possessButton = KeyCode.E;

    //analytics
    //private int puzzleBlocksCollected = 0;

    //movement
    private float speed = 30.0f;
    private float jumpForce = 4000.0f;
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public EntityController currentEntity = null;
    private bool canJump = true;

    //renderer and collider
    [NonSerialized] public SpriteRenderer sr;
    [NonSerialized] public Collider2D _col;
    
    [SerializeField] private GameObject possessionMarkerPrefab;
    private PossessionMarker _possessionMarker;
    [SerializeField] private ContactFilter2D contactFilter;

    // Start is called before the first frame update
    void Start()
    {
        //get references
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        if (GameManager.Instance)
        {
            GameManager.Instance.gameWinEvent.AddListener(GameOver);
        }

        _possessionMarker = Instantiate(possessionMarkerPrefab).GetComponent<PossessionMarker>();
        _possessionMarker.Deactivate();

        //checkpoint
        startPoint = startPointObject.transform;
        endPoint = endPointObject.transform;
        checkPoint = checkPointObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //movement inputs
        horizontalInput = Input.GetAxis("Horizontal");

        //entity possession check
        CheckForEntities();
        //move position to entity if we're possessing one
        if (currentEntity)
        {
            transform.position = currentEntity.transform.position;
        }
        
        //jump
        if (Input.GetKeyDown(jumpButton) && !jumpInputPrevious)
        {
            if (currentEntity)
            {
                currentEntity.Jump();
            }
            jumpInputPrevious = true;
            Jump();
        }
        else
        {
            jumpInputPrevious = false;
        }

    }

    private void FixedUpdate()
    {
        //movement

        //move current entity
        if (currentEntity)
        {
            currentEntity.Move(horizontalInput);
        }
        //normal movement
        else
        {
            //move rigidbody
            Vector2 vel = rb.velocity;
            vel.x = horizontalInput * speed;
            rb.velocity = vel;
        }
    }
    
    public virtual void Jump()
    {
        if (canJump)
        {
            AudioManager.instance.Play("Jump");
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void CheckForEntities()
    {
        Collider2D possessTarget = null;
        if(currentEntity==null){
            //check if we're overlapping entity, player is on IgnoreRaycast layer so it doesn't get picked up
            Collider2D[] entities = new Collider2D[10];
            Physics2D.OverlapCircle((Vector2)transform.position, 8.0f, contactFilter, entities);
            foreach (Collider2D entity in entities)
            {
                //if we hit something
                if (entity)
                {
                    //check if it has entity tag
                    if (entity.CompareTag("Entity") || entity.CompareTag("PuzzleBlock"))
                    {
                        //checks and gets the closest possessible entity
                        if (possessTarget)
                        {
                            if (Math.Abs(Vector3.Distance(entity.transform.position, transform.position)) <
                                Math.Abs(Vector3.Distance(possessTarget.transform.position, transform.position)))
                            {
                                possessTarget = entity;
                            }
                        }
                        else
                        {
                            possessTarget = entity;
                        }
                    }

                    //analytics
                    // if(entity.CompareTag("PuzzleBlock")){
                    //         Debug.Log("Puzzle Blocks Collected: " + puzzleBlocksCollected);

                    //         AnalyticsEvent.Custom("PuzzleBlocksCollected", new Dictionary<string, object>
                    //         {
                    //             { "PuzzleBlocksCollected", puzzleBlocksCollected }
                    //         });
                    // }
                }
            }
        }
        else
        {
            possessTarget = currentEntity.gameObject.GetComponent<Collider2D>();
        }
        if (possessTarget)
        {
            _possessionMarker.Activate(possessTarget.transform.position, possessTarget.gameObject.GetComponent<EntityController>().markerScale * possessTarget.transform.localScale.x);      
        }
        else
        {
            _possessionMarker.Deactivate();
        }

        //if we detect a possess button press, possess
        if (Input.GetKeyDown(possessButton))
        {
            //unpossess if we are already possessing
            if (currentEntity)
            {
                UnPossess();
            }
            //possess if we have a target to possess
            else if (possessTarget)
            {   
                if (possessTarget.CompareTag("PuzzleBlock"))
                {
                    float puzzleTime = Time.time; // Capture the time
                    
                    Debug.Log("PuzzleBlock possessed at time: " + puzzleTime);
                    if (GameManager.Instance)
                    {
                        GameManager.Instance.CalculatePosessionCount("PuzzleBlock");
                        
                    }
                }
                
                Possess(possessTarget.GetComponent<EntityController>());
            }
        }
    }

    public void Possess(EntityController entity)
    {
        if (!entity.canBePossessed)
                return;
        //player sprite invisible
        //collider disabled
        //current entity set
        _possessionMarker.isPossessed = true;
        entity.OnPossess(this);
        currentEntity = entity;
        sr.enabled = false;
        _col.enabled = false;
        AudioManager.instance.Play("Possess");
    }

    public void UnPossess()
    {
        if (currentEntity)
        {
            //set position after possessing
            Vector3 pos = currentEntity.transform.position;
            Collider2D entityCollider = currentEntity.GetComponent<Collider2D>();
            float offsetX = 3 * currentEntity.markerScale;
            float offsetY = 3 * currentEntity.markerScale;
            
            //get bounding box of collider if it has one
            if (entityCollider)
            {
                Vector3 boundSize = entityCollider.bounds.size;
                offsetX = boundSize.x / 2 + 3;
                offsetY = boundSize.y / 2 + 3;
            }

            //move position to the left side of the entity
            pos.x -= offsetX;

            //if colliding with something, move position to the right side
            if (Physics2D.OverlapCircle(pos, 2))
            {
                pos.x += 2 * offsetX;
            }
            //if it's still colliding, try going directly up
            if(Physics2D.OverlapCircle(pos, 2))
            {
                pos.x -= offsetX;
                pos.y += offsetY;
            }

            transform.position = pos;
            currentEntity.OnUnPossess(this);
        }
        
        rb.velocity = Vector2.zero;
        
        //player sprite visible
        //collider enabled
        //current entity cleared

        
        sr.enabled = true;
        _possessionMarker.isPossessed = false;
        currentEntity = null;
        _col.enabled = true;
        AudioManager.instance.Play("UnPossess");
        
    }

    public bool IsPossessing()
    {
        if (currentEntity)
        {
            return true;
        }
        return false;
    }
    //checkpoint
    public void ReachedCheckpoint(Vector3 position)
    {
        if (!hasReachedCheckpoint)
        {
            GameObject notif = Instantiate(checkpointNotif, position, Quaternion.identity);
            Destroy(notif, 3);
        }
        
        hasReachedCheckpoint = true;
        if(!hasReachedCheckpoint){
            hasReachedCheckpoint = true;
            timeToReachCheckpoint = Time.time;
        }
        
    }

    // Call this method when the player reaches the end point.
    public void ReachedEndPoint()
    {
        hasReachedEndPoint = true;
    }

    public void Die(string s)
    {
        //checkpoint
        AudioManager.instance.Play("Hurt");
        if(deathCount<1){
            Time.timeScale = 0.0f;
            GameOver();
            GameManager.Instance.GameLose(s);
        }else{
            deathCount--;
            // Debug.Log("not yet ded");
            if (deathCount == 2)
            {
                heart3.enabled = false; 
            }
            else if (deathCount == 1)
            {
                heart2.enabled = false; 
            }
            else if (deathCount == 0)
            {
                heart1.enabled = false; 
            }
            if(hasReachedCheckpoint){
                transform.position = checkPoint.position;
            
            }else{
                transform.position = startPoint.position;
            }
        }

        
        // Time.timeScale = 0.0f;
        // GameOver();
        // GameManager.Instance.GameLose(s);

    }

    public void GameOver()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 15) && !canJump)
        {
            canJump = true;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canJump && Physics2D.Raycast(transform.position, Vector2.down, 15) && !canJump && rb.velocity.y <= 0)
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("CheckPoint"))
        {
            // Do something when an enemy enters the trigger.
            Debug.Log("Enemy entered the trigger!");
            ReachedCheckpoint(other.transform.position);
        }
    }
}
