using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //inputs
    private float horizontalInput;
    private bool jumpInputPrevious = false;

    //buttons in case we want to change them
    [SerializeField] private KeyCode jumpButton;
    [SerializeField] private KeyCode possessButton;

    //movement
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 50.0f;
    private Rigidbody2D rb;
    private EntityController currentEntity = null;

    //renderer and collider
    private SpriteRenderer sr;
    private Collider2D col;

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
        rb.AddForce(new Vector2(0.0f, jumpForce));
    }

    private void CheckForEntities()
    {
        //check if we're overlapping entity, player is on IgnoreRaycast layer so it doesn't get picked up
        Collider2D[] entities = new Collider2D[5];
        ContactFilter2D contactFilter = new ContactFilter2D();
        Physics2D.OverlapCircle((Vector2)transform.position, 2.0f, contactFilter.NoFilter(), entities);

        Collider2D possessTarget = null;
        foreach (Collider2D entity in entities)
        {
            //if we hit something
            if (entity)
            {
                //check if it has entity tag
                if (entity.CompareTag("Entity") || entity.CompareTag("Fired"))
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
            }
        }
        
        if (possessTarget)
        {
            //if we detect a possess button press, possess
            if (Input.GetKeyDown(possessButton))
            {
                if (currentEntity)
                {
                    UnPossess();
                } else
                {
                    Possess(possessTarget.GetComponent<EntityController>());
                }
            }
        }
    }

    private void Possess(EntityController entity)
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
    }

    void GameOver()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
}