using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextGenerator : MonoBehaviour {
    [Multiline]
    public string textToDisplay;
    TextMesh textMesh;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        string[] tutorialTexts = textToDisplay.Split(' ');
        textMesh.text = "";
        for(int i = 0; i < tutorialTexts.Length; i++)
        {
            tutorialTexts[i] = TextToReturn(tutorialTexts[i]);
            textMesh.text += " " + tutorialTexts[i];
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    private string TextToReturn(string text)
    {
        if (Input.GetJoystickNames().Length < 1)
        {
            return text;
        }
        else if (Input.GetJoystickNames().Length >= 1 )
        {
            if(Input.GetJoystickNames()[0] == "")
            {
                return text;
            }
            if (text.ToLower() == "j")
            {
                return "X";
            }
            else if (text.ToLower() == "k")
            {
                return "Y";
            }
            else if (text.ToLower() == "space")
            {
                return "LT";
            }
            else if (text.ToLower() == "l")
            {
                return "B";
            }
            else
                return text;
        }
        else
        {
            return text;
        }
    }
}
