using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    bool interacted = false;
    [SerializeField]GameObject interactHint;
    MusicDirector musicDirector;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        musicDirector = MusicDirector.instance;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInRange();
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
        musicDirector.StopAllMusicFadeOut(
            new string[] { "Cello", "Pad", "GuzhengFX", "XiaoDa", "ChiBa", "Chapter0" }
            );
    }

    public override void OnClose()
    {
        isClose = true;
    }
}
