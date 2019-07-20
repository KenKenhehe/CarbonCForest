using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitStateToggler : Interactable {
    public GameObject doorToOpen;
    public GameObject objectToDestory;
    public GameObject objectToActivate;
    public Text interactionHintText;
    bool canPush = false;


    public GameObject soundManager;
    public GameObject BossEnable;
	// Use this for initialization
	void Start () {
        interactionHintText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            canPush = true;
            if (doorToOpen.GetComponent<SceneSwitchHandler>().doorOpened == false)
            {
                interactionHintText.text = "Press F to Interact";
            }
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
        SoundFXHandler.instance.Play("ButtonPress");
        base.Interact();
        doorToOpen.GetComponent<SceneSwitchHandler>().doorOpened = true;
        doorToOpen.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        interactionHintText.text = "Seems some door has opened...";
        StartCoroutine(ClearText());
        objectToActivate.SetActive(true);
        DoorController[] doors = FindObjectsOfType<DoorController>();
        foreach (DoorController door in doors)
        {
            if (door.gameObject.name == "DoorOpenAfterPress")
            {
                door.canOpen = true;
            }
        }

        FindObjectOfType<InfiniteSpawnTrigger>().canStopSpawn = true;
        if (FindObjectOfType<SoundManager>() == null)
        {
            Instantiate(soundManager);
        }
        BossEnable.SetActive(true);
        Destroy(objectToDestory);
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(10);
        interactionHintText.text = "";
    }
}
