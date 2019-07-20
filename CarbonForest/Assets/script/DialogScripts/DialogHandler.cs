using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour {
    public Text dialogText;
    private Queue<string> sentences;
    public float dialogFreq;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startDialogue(Dialog dialog)
    {
        sentences.Clear();

        foreach(string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(nextSentence());
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
    }

    void EndDialog()
    {
        dialogText.text = "";
    }

    public IEnumerator nextSentence()
    {
        while(sentences.Count != 0)
        {
            DisplayNextSentence();
            yield return new WaitForSeconds(dialogFreq);
        }
    }
}
