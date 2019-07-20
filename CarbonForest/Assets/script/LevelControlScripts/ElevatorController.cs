using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : Interactable {
    public float moveSpeed;
    public float buttomY;
    public float upY;
    public bool atButtom;
    public Color glassColor;

    Rigidbody2D rb2d;
    Vector2 dir;
    CameraControl cameraControl;
    SpriteRenderer glassRenderer;
    Animator animator;
    
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        dir = new Vector2(0, -1);
        cameraControl = FindObjectOfType<CameraControl>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y >= upY && isActive == false)
        {
            FindObjectOfType<DoorController>().SetToCanOpen("DoorOpenAfterLiftReach");
        }
	}

    private void FixedUpdate()
    {
        MoveElevator();
    }

    public void MoveElevator()
    {
        animator.SetBool("Active", isActive);
        if (isActive && transform.position.y > buttomY && dir.y < 0)
        {
            transform.Translate(dir * Time.deltaTime * moveSpeed);
            if(transform.position.y <= buttomY)
            {
                isActive = false;
            }
        }
        if(isActive && transform.position.y < upY && dir.y > 0)
        {
            transform.Translate(dir * Time.deltaTime * moveSpeed);
            //rb2d.velocity = dir * Time.deltaTime * moveSpeed;
            if (transform.position.y >= upY)
            {
                isActive = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    public void ActivateElevator()
    {
        isActive = true;
        dir *= -1;
        
    }

    public override void Interact()
    {
        base.Interact();
        cameraControl.camDepth = 0;
        if (isActive == false)
        {
            ActivateElevator();
        }
        
    }

    public override void OnClose()
    {
        base.OnClose();
    }




}
