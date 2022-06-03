using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoorHandler : Interactable
{
    [HideInInspector]
    public bool CanOpen;

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
            boxCollider2D.isTrigger = true;

            //play door open animation
            TerminalAnimator.SetTrigger("Unlock");
            animator.SetTrigger("Open");

        }

        else
        {
            print("Door locked, need key card...");

            //play lock animation
            TerminalAnimator.SetTrigger("Locked");

            // play lock sound
        }
    }

}
