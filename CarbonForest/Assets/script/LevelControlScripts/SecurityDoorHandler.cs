using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoorHandler : Interactable
{
    [HideInInspector]
    public bool CanOpen;

    public static SecurityDoorHandler instance;
    BoxCollider2D boxCollider2D;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        CanOpen = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
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

        }

        else
        {
            print("Door locked, need key card...");

            //play lock animation

            // play lock sound
        }
    }

}
