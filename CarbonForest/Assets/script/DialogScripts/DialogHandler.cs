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


    PlayerGeneralHandler player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerGeneralHandler>();
        sentences = new Queue<string>();
        continueButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact"))
        {
            DisplayNextSentence();
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
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            StopAllCoroutines();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(typeSentence(sentence));
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
        dialogText.text = "";
        foreach(char c in s.ToCharArray())
        {
            dialogText.text += c;
            if (dialogText.text.ToCharArray().Length %5 == 0)
            {
                print("Type Sound");
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
    }

    public IEnumerator nextSentence()
    {
        while(sentences.Count != 0)
        {
            DisplayNextSentence();
            
            yield return new WaitForSeconds(dialogFreq);     
        }
        EndDialog();
    }
}
