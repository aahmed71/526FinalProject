using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            AudioManager.instance.Play("Jump");
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
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void CheckForEntities()
    {
        //check if we're overlapping entity, player is on IgnoreRaycast layer so it doesn't get picked up
        Collider2D[] entities = new Collider2D[10];
        Physics2D.OverlapCircle((Vector2)transform.position, 8.0f, contactFilter, entities);
        Collider2D possessTarget = null;
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
                        GameManager.Instance.BlockPossessTime(puzzleTime);
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
        entity.OnPossess(this);
        currentEntity = entity;
        sr.enabled = false;
        _col.enabled = false;
        AudioManager.instance.Play("Possess");
    }

    public void UnPossess()
    {
        //set position after possessing
        Vector3 pos = transform.position;
        pos.x -= 3 * currentEntity.markerScale;
        pos.y += 3 * currentEntity.markerScale;
        if (Physics2D.OverlapCircle(pos, 3))
        {
            pos.x += 6 * currentEntity.markerScale;
        }
        if(Physics2D.OverlapCircle(pos, 3))
        {
            pos.x = transform.position.x;
            pos.y += 5;
        }
        transform.position = pos;
        rb.velocity = Vector2.zero;
        
        //player sprite visible
        //collider enabled
        //current entity cleared

        currentEntity.OnUnPossess(this);
        sr.enabled = true;
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

    public void Die(string s)
    {
        //checkpoint
        // if(deathCount<1){
        //     Time.timeScale = 0.0f;
        //     GameOver();
        //     GameManager.Instance.GameLose(s);
        // }else{
        //     deathCount--;
        //     Debug.Log("not yet ded");
        // }

        AudioManager.instance.Play("Hurt");
        Time.timeScale = 0.0f;
        GameOver();
        GameManager.Instance.GameLose(s);

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

        if (col.transform.CompareTag("CheckPoint"))
        {
            Debug.Log("checkpoint found");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canJump && Physics2D.Raycast(transform.position, Vector2.down, 15))
        {
            canJump = true;
        }
    }
}
