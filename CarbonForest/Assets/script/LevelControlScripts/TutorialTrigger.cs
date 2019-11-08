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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            if(gameObject.tag == "ArcherTutorial")
            {
                
                tutorialManager.startBlockTutorial();
                Destroy(gameObject, .1f);

            }
        }
    }

    public void DisableMovementControl(GameObject player)
    {

    }

    public void DisableAttack(GameObject player)
    {

    }
}
