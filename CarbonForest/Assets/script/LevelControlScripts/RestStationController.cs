using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStationController : Interactable {

    bool doorHasOpen = false;

    private void Update()
    {
        CheckIfInRange();
    }

    public override void Interact()
    {
        base.Interact();
        if(doorHasOpen == false)
        {
            print("Station interact");
            PlayerGeneralHandler player = FindObjectOfType<PlayerGeneralHandler>();
            GetComponent<BoxCollider2D>().enabled = false;
            doorHasOpen = true;
            //Saver.Save(GameStateHolder.instance);
        }
    }
}
