using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchHandler : Interactable {
    public bool canExit = false;
    public bool doorOpened = false;
    public bool bossDefeated = false;
    public GameObject defeatBossText;
    [Header("how many second after when want to switch scene")]
    public float secondPassToSwitchScene;
	// Use this for initialization
	void Start () {
        
	}

    private void Update()
    {
        if(FindObjectOfType<BossController>() == null)
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
        if(doorOpened == true)
        {
            canExit = true;
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (canExit == true && bossDefeated == true)
        {           
            StartCoroutine(switchToNextScene());
            if (FindObjectOfType<SoundManager>() != null)
            {
                StartCoroutine(FindObjectOfType<SoundManager>().FadeOut(.05f));
            }
        }
        else if(bossDefeated == false)
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
