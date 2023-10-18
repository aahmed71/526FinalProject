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

    //buttons in case we want to change them
    [SerializeField] private KeyCode jumpButton;
    [SerializeField] private KeyCode possessButton;

    //analytics
    private int puzzleBlocksCollected = 0;

    //movement
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 500.0f;
    [NonSerialized] public Rigidbody2D rb;
    [NonSerialized] public EntityController currentEntity = null;
    private bool canJump = true;

    //renderer and collider
    [NonSerialized] public SpriteRenderer sr;
    [NonSerialized] public Collider2D col;

    public GameObject gameControl;
    [SerializeField] private GameObject possessionMarkerPrefab;
    private PossessionMarker _possessionMarker;
    [SerializeField] private ContactFilter2D contactFilter;

    // Start is called before the first frame update
    void Start()
    {
        //get references
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
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
        Vector2 movement = new Vector2(horizontalInput, 0.0f);

        //move current entity
        if (currentEntity)
        {
            currentEntity.Move(horizontalInput);
        }
        //normal movement
        else
        {
            //move rigidbody
            rb.position += movement * (speed * Time.deltaTime);
        }
    }
    
    public virtual void Jump()
    {
        if (canJump)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce));
            canJump = false;
        }
    }

    private void CheckForEntities()
    {
        //check if we're overlapping entity, player is on IgnoreRaycast layer so it doesn't get picked up
        Collider2D[] entities = new Collider2D[10];
        int x = Physics2D.OverlapCircle((Vector2)transform.position, 8.0f, contactFilter, entities);
        Debug.Log(x);
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

        if (possessTarget && !currentEntity)
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
        col.enabled = false;
    }

    public void UnPossess()
    {
        //player sprite visible
        //collider enabled
        //current entity cleared

        currentEntity.OnUnPossess(this);
        sr.enabled = true;
        currentEntity = null;
        col.enabled = true;
        
        //set position after possessing
        Vector3 pos = transform.position;
        pos.x -= 1;
        pos.y += 3;
        if (Physics2D.OverlapPoint(pos))
        {
            pos.x += 2;
        }
        transform.position = pos;
        rb.velocity = Vector2.zero;
    }

    public bool IsPossessing()
    {
        if (currentEntity)
        {
            return true;
        }
        return false;
    }

    public void Die()
    {
        Time.timeScale = 0.0f;
        GameOver();
        gameControl.SetActive(true);
        GameManager.Instance.Background.SetActive(true);
        GameManager.Instance.PauseButton.SetActive(false);
        GameManager.Instance.RestartButton.SetActive(true);
        GameManager.Instance.Lose.SetActive(true);

    }

    public void GameOver()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.position.y < transform.position.y && !canJump)
        {
            canJump = true;
        }
    }
}
