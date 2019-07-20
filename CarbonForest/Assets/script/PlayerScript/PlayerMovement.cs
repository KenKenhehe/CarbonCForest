﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public GameObject dodgeFXLeft;
    public GameObject dodgeSparkFX;

    public float speed;
    public float dodgeSpeed;
    public float walkSpeed;
    public float jumpHeight;
    public float horizontalMovement;
    public float fallMultiplier = 1.5f;
    public bool onGround = true;
    
    public bool dodging = false;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public bool facingRight = true;
    public bool canJump = true;
    Rigidbody2D rb2d;
    public float screenMinX = -10;
    public float screenMaxX = 13;

    PlayerAttack playerAttack;
    BlockController blockController;
    CounterAttackController counterAttack;

    private float dodgeTime;
    public float startDodgeTime;

    private GameObject LeftBound;
    private GameObject RightBound;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
        blockController = GetComponent<BlockController>();
        counterAttack = GetComponent<CounterAttackController>();
        dodgeTime = startDodgeTime;
        LeftBound = GameObject.FindGameObjectWithTag("LeftBound");
        RightBound = GameObject.FindGameObjectWithTag("RightBound");
	}
	
	// Update is called once per frame
	void Update () {     
        MovementInput();
        //OnlyMoveBetween(LeftBound.transform.position.x, RightBound.transform.position.x);
        
	}

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(horizontalMovement * speed * Time.deltaTime, rb2d.velocity.y);

        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
        OnlyMoveBetween(LeftBound.transform.position.x, RightBound.transform.position.x);

        HandleDodge();
    }

    void OnlyMoveBetween(float minX, float maxX)
    {
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }

    void MovementInput()
    {
        if (onGround == true)
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
        }
        
        //rb2d.velocity = new Vector2(horizontalMovement * speed * Time.deltaTime, rb2d.velocity.y);

        if (GetComponent<BlockController>().blocking == false)
        {
            NormalMovement();
        }
        else if(blockController.blocking == true)
        {
            animator.SetBool("isWalking", false);
            BlockingMovement();
        }

        /*if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * fallMultiplier * Physics2D.gravity.y * Time.deltaTime;
        }*/

        //HandleDodge();
    }

    void HandleDodge(){
        if (dodging == false)
        {
           if (Input.GetButtonDown("Fire2")
           && playerAttack.attacking == false
           && GetComponent<BlockController>().blocking == false)
           {
                DodgeFX();
                dodging = true;
           }
        }
        else
        {
            if (dodgeTime <= 0)
            {
                dodging = false;
                dodgeTime = startDodgeTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                dodgeTime -= Time.deltaTime;
                rb2d.velocity = new Vector2(
                facingRight == true ? (dodgeSpeed) * Time.deltaTime : -(dodgeSpeed) * Time.deltaTime,
                rb2d.velocity.y
                );
            }
        }  
}

    void NormalMovement()
    {
        //walk animation
        if (horizontalMovement != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else if (horizontalMovement == 0)
        {
            animator.SetBool("isWalking", false);
        }
        //facing which way
        if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
        else if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
    }

    void BlockingMovement()
    {
        if (counterAttack.countering == false)
        {
            if (facingRight == true && horizontalMovement != 0)
            {
                if (horizontalMovement > 0)
                {
                    animator.SetBool("DefendWalkBackward", false);
                    animator.SetBool("DefendWalkForward", true);
                }
                else if (horizontalMovement < 0)
                {
                    animator.SetBool("DefendWalkForward", false);
                    animator.SetBool("DefendWalkBackward", true);
                }
            }
            else if (facingRight == false && horizontalMovement != 0)
            {

                if (horizontalMovement < 0)
                {
                    animator.SetBool("DefendWalkForward", true);
                    animator.SetBool("DefendWalkBackward", false);
                }
                else if (horizontalMovement > 0)
                {
                    animator.SetBool("DefendWalkForward", false);
                    animator.SetBool("DefendWalkBackward", true);
                }
            }

            if (horizontalMovement == 0)
            {
                animator.SetBool("DefendWalkForward", false);
                animator.SetBool("DefendWalkBackward", false);
            }
        }
        else if(counterAttack.countering)
        {
            animator.SetBool("DefendWalkForward", false);
            animator.SetBool("DefendWalkBackward", false);
        }

    }

    void DodgeFX()
    {
        GameObject sFX = Instantiate(
            dodgeSparkFX, 
            new Vector3(transform.position.x, transform.position.y - 1, transform.position.z),
            Quaternion.Euler(new Vector3(-15, facingRight ? -90 : 90, 0)), transform
            );
        if(rb2d.velocity.y < 0)
        {
            Destroy(sFX);
        }
        Instantiate(
            dodgeFXLeft, new Vector3(transform.position.x - (facingRight ? -1f : 1f),
                transform.position.y,
                transform.position.z),
                Quaternion.Euler(new Vector3(0, 0, (facingRight ? 180 : 0)))
                );

        animator.SetTrigger("Dodging");
    }

    IEnumerator DoudgeEffect()
    {
        dodging = true;
        if (horizontalMovement == 0)
        {
            rb2d.velocity = new Vector2(
                facingRight == true ? (dodgeSpeed) * Time.deltaTime : -(dodgeSpeed) * Time.deltaTime, 
                rb2d.velocity.y
                );
        }
        else {
            rb2d.velocity = new Vector2((dodgeSpeed ) * horizontalMovement * Time.deltaTime, rb2d.velocity.y);
        }
        yield return new WaitForSeconds(1f);
        speed = walkSpeed;
        dodging = false;
    }

    
    void AdjustBlockFacing()
    {
        spriteRenderer.flipX = !(spriteRenderer.flipX);
    }
}
