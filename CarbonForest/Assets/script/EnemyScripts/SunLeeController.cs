using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLeeController : EnemyCQC
{
    bool inBikeMode = true;
    public static SunLeeController instance;
    [SerializeField] GameObject smokeObj;

    public Dialog introDialog;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(instance == null)
        {
            instance = this;
            print("instance attached");
        }
        //Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(inBikeMode)
        {
            BikeBehaviour();
        }
        else
        {
            EnableBehaviour();
        }
    }

   
    void BikeBehaviour()
    {
        //animator.SetTrigger()
    }

    public void ToCombatMode()
    {
        DialogHandler.instance.startDialogue(introDialog);
        DialogHandler.instance.onDialogueEnd = OnDialogFinished;
    }

    public void OnDialogFinished()
    {
        Destroy(smokeObj);
        animator.SetTrigger("ToCombat");
    }
}
