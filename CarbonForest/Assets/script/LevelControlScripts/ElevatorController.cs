using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : Interactable {
    public float moveSpeed;
    public float buttomY;
    public float upY;
    public bool atButtom;
    public Color glassColor;

    public GameObject TextFX;

    public Transform Up;
    public Transform buttom;

    Rigidbody2D rb2d;
    public Vector2 dir = new Vector2(0,-1);
    CameraControl cameraControl;
    Animator animator;
    Animator textFXAnimator;
    
    
	// Use this for initialization
	void Start () 
    {
        if(TextFX != null)
        {
            textFXAnimator = TextFX.GetComponent<Animator>();
        }
        rb2d = GetComponent<Rigidbody2D>();
        cameraControl = FindObjectOfType<CameraControl>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        EnableBehaviour();
        if (transform.position.y >= Up.position.y && isActive == false)
        {
            if(FindObjectOfType<DoorController>() != null)
                FindObjectOfType<DoorController>().SetToCanOpen("DoorOpenAfterLiftReach");
        }
	}

    private void FixedUpdate()
    {
        CheckIfInRange();
        MoveElevator();
    }

    public void MoveElevator()
    {
        textFXAnimator.SetBool("Activate", isActive);
        animator.SetBool("Active", isActive);
        if (isActive && transform.position.y > buttom.position.y && dir.y < 0)
        {
            transform.Translate(dir * Time.deltaTime * moveSpeed);
            if (transform.position.y <= buttom.position.y)
            {
                isActive = false;
                interacted = false;
            }
        }
        if (isActive && transform.position.y < Up.position.y && dir.y > 0)
        {
            transform.Translate(dir * Time.deltaTime * moveSpeed);
            if (transform.position.y >= Up.position.y)
            {
                isActive = false;
                interacted = false;
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
        //base.Interact();
        cameraControl.camDepth = 0;
        if (isActive == false)
        {
            ActivateElevator();
            interacted = true;
        }
        
    }

    public override void OnClose()
    {
        isClose = true;
    }




}
