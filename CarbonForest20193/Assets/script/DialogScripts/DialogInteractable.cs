using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : Interactable
{
    public Dialog dialog;
    
    public GameObject dialogTipImage;
    public Vector2 scale;
    bool isClose = false;
    DialogHandler dialogHandler;
    // Start is called before the first frame update
    void Start()
    {
        dialogHandler = FindObjectOfType<DialogHandler>();

    }

    private void Update()
    {
        if (isClose == true)
        {
            dialogTipImage.transform.localScale =
               Vector3.Lerp(dialogTipImage.transform.localScale, scale, Time.deltaTime * 10);
            if (dialogTipImage.transform.localScale.x >= scale.x - .2f)
            {
                isClose = false;
            }
        }
        else
        {
            dialogTipImage.transform.localScale =
                Vector3.Lerp(dialogTipImage.transform.localScale, Vector3.zero, Time.deltaTime * 10);
        }
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
