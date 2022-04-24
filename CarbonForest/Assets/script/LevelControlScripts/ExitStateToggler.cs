using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitStateToggler : Interactable {
    public GameObject doorToOpen;
    public GameObject ExitTerminal;
    public GameObject objectToDestory;
    public GameObject objectToActivate;
    bool canPush = false;


    public GameObject soundManager;
    public GameObject BossEnable;

    DoorController[] doors;
    InfiniteSpawnTrigger infiniteSpawnTrigger;

    // Use this for initialization
    void Start () {
        ExitTerminal.GetComponent<InteractableHolo>().enabled = false;
        doors = FindObjectsOfType<DoorController>();
        infiniteSpawnTrigger = FindObjectOfType<InfiniteSpawnTrigger>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckIfInRange();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            canPush = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            canPush = false;
        }
    }

    public override void Interact()
    {
        //Use a coroutine so that this doesn't stall
        StartCoroutine(DoorOpenRoutine());
    }

    IEnumerator DoorOpenRoutine()
    {
        SoundFXHandler.instance.Play("ButtonPress");
        base.Interact();
        doorToOpen.GetComponent<SceneSwitchHandler>().doorOpened = true;
        doorToOpen.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        objectToActivate.SetActive(true);
        foreach (DoorController door in doors)
        {
            if (door.gameObject.name == "DoorOpenAfterPress")
            {
                door.canOpen = true;
            }
        }
        infiniteSpawnTrigger.canStopSpawn = true;
        if (FindObjectOfType<SoundManager>() == null)
        {
            Instantiate(soundManager);
        }
        BossEnable.SetActive(true);
        Destroy(objectToDestory);
        ExitTerminal.GetComponent<InteractableHolo>().enabled = true;
        yield return null;
    }
}
