using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour {
    public Text dialogText;
    public GameObject continueButton;
    private Queue<string> sentences;
    public float dialogFreq;
    public GameObject cinamicFX;
    public Animator animator;

    public delegate void OnDialogueEnd();
    public OnDialogueEnd onDialogueEnd = DefalutDialogueEndBehaviour;

    private static void DefalutDialogueEndBehaviour()
    {
        print("dialogueEnd");
    }

    PlayerGeneralHandler player;
    bool typing = false;
    string currentSentence = "";

    public static DialogHandler instance;

    // Use this for initialization
    void Start () {
        if(instance == null)
        {
            instance = this;
        }
        player = PlayerGeneralHandler.instance;
        sentences = new Queue<string>();
        continueButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact"))
        {
            DisplayNextSentence(typing);
        }
	}

    public void startDialogue(Dialog dialog)
    {
        cinamicFX.GetComponent<Animator>().SetBool("IsOpen", true);
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        player.DeactivateControl();
        player.enabled = false;
        player.GetComponent<Animator>().SetBool("isWalking", false);
        player.GetComponent<Animator>().SetBool("Defending", false);
        player.GetComponent<Animator>().SetBool("DefendWalkForward", false);
        player.GetComponent<Animator>().SetBool("DefendWalkBackward", false);
        DisplayNextSentence(typing);
    }

    public void DisplayNextSentence(bool isTyping)
    {
        typing = false;
        if(sentences.Count == 0 && isTyping == false)
        {
            EndDialog();
            onDialogueEnd();
            StopAllCoroutines();
            return;
        }
        else
        {
            dialogText.text = currentSentence;
        }
        
        StopAllCoroutines();
        if (isTyping == false)
        {
            currentSentence = sentences.Dequeue();
            StartCoroutine(typeSentence(currentSentence));
        }else
        {
            dialogText.text = currentSentence;
        }
    }

    void EndDialog()
    {
        dialogText.text = "";
        cinamicFX.GetComponent<Animator>().SetBool("IsOpen", false);
        animator.SetBool("IsOpen", false);
        player.enabled = true;
        player.ReactivateControl();
    }

    IEnumerator typeSentence(string s)
    {
        typing = true;
        dialogText.text = "";
        foreach(char c in s.ToCharArray())
        {
            dialogText.text += c;
            if (dialogText.text.ToCharArray().Length %5 == 0)
            {
                FindObjectOfType<SoundFXHandler>().Play("Type");
            }
            if(dialogText.text.ToCharArray().Length >= 5)
            {
                continueButton.SetActive(true);
            }
            else
            {
                continueButton.SetActive(false);
            }
            yield return new WaitForSeconds(.02f);
        }
        print("finish");
        typing = false;
    }

    public IEnumerator nextSentence()
    {
        while(sentences.Count != 0)
        {
            DisplayNextSentence(typing);
            
            yield return new WaitForSeconds(dialogFreq);     
        }
        EndDialog();
    }
}
