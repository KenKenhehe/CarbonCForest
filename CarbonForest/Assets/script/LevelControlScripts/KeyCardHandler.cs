using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardHandler : Interactable
{
    public GameObject tip;
    public GameObject KeycardSprite;

    private void Update()
    {
        EnableBehaviour();
        CheckIfInRange();
    }

    public override void Interact()
    {
        print("Keycard interacted");

        KeycardSprite.SetActive(false);
        //play collect animation

        //set door to can open
        SecurityDoorHandler.instance.CanOpen = true;

        if(tip != null)
            PlayerGeneralHandler.instance.DisplayTip(tip, new Vector2(0, 0.4f));

        Destroy(gameObject);
    }
}
