using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryWindowHandler : MonoBehaviour
{
    public GameObject slide1;
    public GameObject slide2;
    TutorialManagerZero tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = TutorialManagerZero.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManagerZero.InTutorial)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                print("ParryTutInput");
            }
        }
    }
}
