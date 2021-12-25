using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    public Dialog dialog;
    
    public GameObject dialogTipImage;
    public Vector2 scale;
    DialogHandler dialogHandler;
    // Start is called before the first frame update
    void Start()
    {
        dialogHandler = FindObjectOfType<DialogHandler>();

    }

    private void Update()
    {
        CheckIfInRange();
        EnableBehaviour();
    }

    public override void Interact()
    {
        base.Interact();
        dialogHandler.startDialogue(dialog);
    }

    public override void OnClose()
    {
        isClose = true;
    }
}
