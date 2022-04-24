using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneEventHandler : MonoBehaviour
{
    public bool StartsWithDialogue = false;
    public GameObject[] objectToPlayAni;
    Animator[] animators;
    public PlayableDirector timelineDirector;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (objectToPlayAni.Length > 0)
        {
            animators = new Animator[objectToPlayAni.Length];
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i] = objectToPlayAni[i].GetComponent<Animator>();
            }
        }
    }

   



    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            TriggerCollider();
        }
    }

    void TriggerEvent()
    {
        PlayerGeneralHandler.instance.DeactivateControl();
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetBool("isWalking", false);
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetBool("Defending", false);
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetBool("DefendWalkForward", false);
        PlayerGeneralHandler.instance.GetComponent<Animator>().SetBool("DefendWalkBackward", false);
        CutSceneEvent();
    }

    public virtual void TriggerCollider()
    {
        TriggerEvent();
        GetComponent<BoxCollider2D>().enabled = false;
    }



    public virtual void CutSceneEvent()
    {
        FindObjectOfType<CameraControl>().camDepth = -2.5f;
        timelineDirector.Play();
    }
}
