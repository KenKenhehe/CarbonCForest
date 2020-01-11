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
                StartCoroutine(DisableMovementControlForAwhile(collision.gameObject));
                tutorialManager.startBlockTutorial();
                GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject, 5f);
            }
            else if(gameObject.tag == "TeachParry")
            {
                tutorialManager.StartParryTutorial();
                Destroy(gameObject, 2);
            }
        }
    }

    public void DisableMovementControl(GameObject player)
    {
        
    }

    public void DisableAttack(GameObject player)
    {

    }

    IEnumerator DisableMovementControlForAwhile(GameObject player)
    {
        player.GetComponent<PlayerGeneralHandler>().DeactivateControl();
        player.GetComponent<Animator>().SetBool("isWalking", false);
        player.GetComponent<Animator>().SetBool("Defending", false);
        player.GetComponent<Animator>().SetBool("DefendWalkForward", false);
        player.GetComponent<Animator>().SetBool("DefendWalkBackward", false);
        yield return new WaitForSeconds(4);
        player.GetComponent<PlayerGeneralHandler>().ReactivateControl();
    }
}
