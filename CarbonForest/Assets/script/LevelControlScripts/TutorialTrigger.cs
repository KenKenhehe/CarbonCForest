using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialManagerZero tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = TutorialManagerZero.instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            if(gameObject.tag == "ArcherTutorial")
            {
                tutorialManager.startBlockTutorial();
                GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject, 5f);
            }
            else if(gameObject.tag == "TeachParry")
            {
                tutorialManager.StartParryTutorial();
                Destroy(gameObject, 2);
            }
            else if(gameObject.tag == "ArrowFall")
            {
                tutorialManager.ArrowFall();
                Destroy(gameObject);
            }
            else if(gameObject.tag == "Dodge")
            {
                tutorialManager.StartDodgeTutorial(collision);
                Destroy(gameObject);
            }
            else if(gameObject.tag == "StopArrowFall")
            {
                tutorialManager.ToggleArrowFall(false);
                Destroy(gameObject);
            }
            else if(gameObject.tag == "ResumeArrowFall")
            {
                tutorialManager.ResumeArrowFall();
                Destroy(gameObject);
            }
        }
    }

}
