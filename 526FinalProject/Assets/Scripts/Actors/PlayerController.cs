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
        GameManager.Instance.gameWinEvent.AddListener(GameOver);
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
            
            //jump
            if (Input.GetKeyDown(jumpButton) && !jumpInputPrevious)
            {
                currentEntity.Jump();
                jumpInputPrevious = true;
            }
            else
            {
                jumpInputPrevious = false;
            }
        }

    }

    private void FixedUpdate()
    {
        //move current entity
        if (currentEntity)
        {
            currentEntity.Move(horizontalInput,0);
        }
        //normal movement
        else
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }

    private void CheckForEntities()
    {
        //check if we're overlapping entity, player is on IgnoreRaycast layer so it doesn't get picked up
        Collider2D entity = Physics2D.OverlapCircle((Vector2)transform.position, 2);

        //if we hit something
        if (entity)
        {
            //check if it has entity tag
            if (entity.CompareTag("Entity") || entity.CompareTag("Fired"))
            {
                //if we detect a possess button press, possess
                if (Input.GetKeyDown(possessButton))
                {
                    Possess(entity.GetComponent<EntityController>());
                }
            }
        }
    }

    private void Possess(EntityController entity)
    {
        //if we're already possessing something, unpossess
        if (currentEntity)
        {
            UnPossess();
        }
        //possess
        else
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
    }

    public void UnPossess()
    {
        //player sprite visible
        //collider enabled
        //current entity cleared
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
