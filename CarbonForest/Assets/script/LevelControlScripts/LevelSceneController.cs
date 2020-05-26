using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSceneController: MonoBehaviour
{
    public GameObject ObjectiveScene;
    public GameObject cutSceneText;
    public GameObject resetText;
    PlayerGeneralHandler player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        cutSceneText.SetActive(false);
        resetText.SetActive(false);
        ObjectiveScene.SetActive(true);
        StartCoroutine(ShowObjectiveSceneForAwhile());
    }

    IEnumerator ShowObjectiveSceneForAwhile()
    {
        player.DeactivateControl();
        yield return new WaitForSeconds(3f);
        cutSceneText.SetActive(true);
        yield return new WaitForSeconds(2f);
        resetText.SetActive(true);
        yield return new WaitForSeconds(3f);
        player.ReactivateControl();
        Destroy(ObjectiveScene);
    }
}
