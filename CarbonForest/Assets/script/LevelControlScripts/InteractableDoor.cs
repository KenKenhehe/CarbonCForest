using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    bool isClose = false;
    bool interacted = false;
    [SerializeField]GameObject interactHint;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isClose == true && interacted == false)
        {
            interactHint.SetActive(true);
            isClose = false;
        }
        else
        {
            interactHint.SetActive(false);
        }
    }

    public override void Interact()
    {
        animator.SetTrigger("Open");
        boxCollider2D.enabled = false;
        interacted = true;
    }

    public override void OnClose()
    {
        isClose = true;
    }
}
