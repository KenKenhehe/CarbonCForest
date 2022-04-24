using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeadPopupController : MonoBehaviour
{
    public Image BG;
    public static PlayerDeadPopupController instance;
    Animator BgAnimator;
    Animator KeepGoingAnimator;
    public GameObject KeepGoingUI;
    public GameObject RestartUI;
    public GameObject ReturnToMenuUI;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        BgAnimator = BG.gameObject.GetComponent<Animator>();
        KeepGoingAnimator = KeepGoingUI.GetComponent<Animator>();
    }

    public void ToDeadPopup(bool needRestart)
    {
        BG.gameObject.SetActive(true);
        ReturnToMenuUI.SetActive(true);
        if (needRestart)
        {
            KeepGoingUI.SetActive(false);
            RestartUI.SetActive(true);
        }
        else
        {
            RestartUI.SetActive(false);
            KeepGoingUI.SetActive(true);
        }
    }

    public void selectKeepGoing()
    {
        
    }

    public void selectRestart()
    {

    }

    public void selectReturn()
    {

    }

}
