using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchHandler : Interactable
{
    public bool canExit = false;
    public bool doorOpened = false;
    public bool bossDefeated = false;
    public GameObject defeatBossText;
    [Header("how many second after when want to switch scene")]
    public float secondPassToSwitchScene;
    public GameObject exitAnimObj;
    PlayerGeneralHandler player;
    // Use this for initialization
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        if (exitAnimObj != null)
        {
            exitAnimObj.GetComponent<InteractableHolo>().enabled = false;
        }
    }

    private void Update()
    {
        CheckIfInRange();
        EnableBehaviour();
        if (FindObjectOfType<BossController>() == null)
        {
            bossDefeated = true;
        }
        else
        {
            bossDefeated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (doorOpened == true)
        {
            canExit = true;
            if (exitAnimObj != null)
            {
                exitAnimObj.GetComponent<InteractableHolo>().enabled = true;
            }
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (canExit == true && bossDefeated == true)
        {
            StartCoroutine(switchToNextScene());
            player.DeactivateControl();
            if (FindObjectOfType<SoundManager>() != null)
            {
                StartCoroutine(FindObjectOfType<SoundManager>().FadeOut(.05f));
            }
            if (exitAnimObj != null && exitAnimObj.GetComponent<InteractableHolo>() != null)
            {
                exitAnimObj.GetComponent<InteractableHolo>().Interact();
            }
        }
        else if (bossDefeated == false)
        {
            Instantiate(defeatBossText);
        }

    }

    IEnumerator switchToNextScene()
    {
        yield return new WaitForSeconds(secondPassToSwitchScene);
        FindObjectOfType<LevelFader>().GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
