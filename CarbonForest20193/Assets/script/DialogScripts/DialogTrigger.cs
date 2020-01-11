using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {
    public Dialog dialog;
    DialogHandler dialogHandler;
    private void Start()
    {
        dialogHandler = FindObjectOfType<DialogHandler>();
    }
    public void TriggerDialogue()
    {
        dialogHandler.startDialogue(dialog);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            TriggerDialogue();
            Destroy(gameObject, 1);
        }
    }
}
