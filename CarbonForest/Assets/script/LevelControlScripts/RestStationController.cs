using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStationController : Interactable {

    bool doorHasOpen = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        base.Interact();
        if(doorHasOpen == false)
        {
            PlayerGeneralHandler player = FindObjectOfType<PlayerGeneralHandler>();
            GetComponent<BoxCollider2D>().enabled = false;
            doorHasOpen = true;
            Saver.Save(player);
        }
    }
}
