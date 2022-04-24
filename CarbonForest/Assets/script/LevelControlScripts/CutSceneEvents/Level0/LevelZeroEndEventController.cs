using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelZeroEndEventController : CutSceneEventHandler
{
    public GameObject fadeOutCanvas;
    public GameObject BossShow;
    Animator animator;
    public Dialog dialog;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    public override void Init()
    {
        BossShow.SetActive(false);
        base.Init();
        timelineDirector.played += PrepareEndPose;
        timelineDirector.stopped += TimeLineEnd;
        animator = fadeOutCanvas.GetComponent<Animator>();
    }

    public override void CutSceneEvent()
    {
        base.CutSceneEvent();
        CameraControl.instance.offsetY = .35f;
    }

    public void PrepareEndPose(PlayableDirector obj)
    {
        animator.SetBool("IsFadeOut", true);
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetTrigger("BlockFail");
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetBool("BlockFailIdle", true);
    }

    public void TimeLineEnd(PlayableDirector obj)
    {
        DialogHandler.instance.startDialogue(dialog);
        DialogHandler.instance.onDialogueEnd = () =>
        {
            print("InTImeline");
            FindObjectOfType<SceneSwitchHandler>().Interact();
        };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            TriggerCollider();
        }
    }

    public override void TriggerCollider()
    {
        base.TriggerCollider();
        BossShow.SetActive(true);
    }
}
