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

	// Use this for initialization
	void Start () 
    {
        if(GetComponentInChildren<TextMesh>() != null)
        {
            
            Init();
        }
        print(gameObject.name + ": " + GetComponentInChildren<TextMesh>());

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
        CheckIfInRange();
        if (interacted == true && HologramSprite.transform.localScale.x <= maxScale.x - .2f)
        {
            HologramSprite.transform.localScale =
                Vector3.Lerp(HologramSprite.transform.localScale, maxScale, Time.deltaTime * 5);
        }
        else if (interacted == false)
        {
            HologramSprite.transform.localScale =
                Vector3.Lerp(HologramSprite.transform.localScale, Vector3.zero, Time.deltaTime * 5);

        }
        EnableBehaviour();
    }

    public override void Interact()
    {
        if(interacted == false)
        {
            interacted = true;
            textToDisplay.text = interactedText;
            textToDisplay.fontSize = sizeAfter;
        }
        else
        {
            textToDisplay.fontSize = sizeBefore;
            textToDisplay.text = onCloseText;
            interacted = false;
        }
    }

    public override void OnClose()
    {
        isClose = true;
    }
}
