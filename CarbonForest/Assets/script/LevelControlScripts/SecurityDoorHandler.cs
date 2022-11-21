using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoorHandler : Interactable
{
    [HideInInspector]
    public bool CanOpen;

    public bool isExitDoor;
    public GameObject exit;

    public static SecurityDoorHandler instance;
    BoxCollider2D boxCollider2D;

    public Animator TerminalAnimator;
    public Animator animator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        CanOpen = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        if (isExitDoor)
        {
            boxCollider2D.isTrigger = true;
        }
        exit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        CheckIfInRange();
    }

    public override void Interact()
    {
        if(CanOpen == true)
        {
            print("Opening door");
            TerminalAnimator.SetTrigger("Unlock");
            animator.SetTrigger("Open");
            if (!isExitDoor)
            {
                boxCollider2D.isTrigger = true;

                //play door open animation
                boxCollider2D.enabled = false;
            }
            else
            {
                exit.SetActive(true);
                boxCollider2D.enabled = false;
            }

        }
        else
        {
            print("Door locked, need key card...");

            //play lock animation
            TerminalAnimator.SetTrigger("Locked");

            //TODO play lock sound
        }
    }

}
