using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHolo : Interactable {

    public int sizeAfter = 40;
    public int sizeBefore = 60;
    public GameObject HologramSprite;
    [TextArea]
    public string onCloseText;
    [TextArea]
    public string interactedText;
    public Vector3 maxScale;
    public Vector3 middScale;

    TextMesh textToDisplay;

    float smoothness = 10;
    public bool isInteracted = false;

	// Use this for initialization
	void Start () 
    {
        if(GetComponentInChildren<TextMesh>() != null)
        {
            Init();
        }
	}

    public void Init()
    {
        middScale = new Vector3(1, .75f, 1);
        textToDisplay = GetComponentInChildren<TextMesh>();
        textToDisplay.text = onCloseText;
        textToDisplay.fontSize = sizeBefore;
    }
	
	// Update is called once per frame
	void Update () {

        if (isInteracted == true)
        {
            HologramSprite.transform.localScale = 
                Vector3.Lerp(HologramSprite.transform.localScale, maxScale, Time.deltaTime * 10);
        }
        else
        {
            EnableBehaviour();
        }
        //else if(isClose == true)
        //{
        //    HologramSprite.transform.localScale =
        //       Vector3.Lerp(HologramSprite.transform.localScale, middScale, Time.deltaTime * 10);
        //    if(HologramSprite.transform.localScale.x >= middScale.x - .2f)
        //    {
        //        isClose = false;
        //    }
        //}
        
        //else
        //{
        //    HologramSprite.transform.localScale = 
        //        Vector3.Lerp(HologramSprite.transform.localScale, Vector3.zero, Time.deltaTime * 10);
        //}

       
	}

    public override void Interact()
    {
        //base.Interact();
        if(isInteracted == false)
        {
            isInteracted = true;
            textToDisplay.text = interactedText;
            textToDisplay.fontSize = sizeAfter;
        }
        else
        {
            textToDisplay.fontSize = sizeBefore;
            textToDisplay.text = onCloseText;
            isInteracted = false;
        }
    }

    public override void OnClose()
    {
        isClose = true;
    }
}
