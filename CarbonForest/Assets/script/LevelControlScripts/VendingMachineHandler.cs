using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineHandler : Interactable
{
    // Start is called before the first frame update
    public Animator VendingMacAnimator;
    public Animator QRCodeAnimator;

    public GameObject AdditionalHint;

    public GameObject noodle;
    bool hasOpenShelf = false;

    bool hasLeft = false;
    bool hasRecover = false;

    public float cameraDepthWhenFocus = 2;

    float originalCameraDepth;

    private void Start()
    {
        noodle.SetActive(false);
    }

    void Update()
    {
        CheckIfInRange();
        EnableBehaviour();
    }

    public override void Interact()
    {
        if (hasOpenShelf == false)
        {
            StartCoroutine(EnableQRcodeAndVendingMachine());
            interactHint.SetActive(false);
            interactHint = AdditionalHint;
        }
        else
        {
            if (hasRecover == false)
            {
                //play sound
                SoundFXHandler.instance.Play("NoodleAte");
                noodle.SetActive(false);
                hasRecover = true;
                PlayerGeneralHandler.instance.TopUpStatus();
                interacted = true;
            }
        }
    }

    IEnumerator EnableQRcodeAndVendingMachine()
    {
        QRCodeAnimator.SetTrigger("PaySuccess");
        SoundFXHandler.instance.Play("PaySuccess");
        yield return new WaitForSeconds(1);
        //play open shelf sound
        VendingMacAnimator.SetTrigger("OpenShelf");
        hasOpenShelf = true;
        yield return new WaitForSeconds(0.25f);
    }

    public void ShowNoodle()
    {
        noodle.SetActive(true);
    }

    public override void OnClose()
    {
        isClose = true;
        hasLeft = false;
        print("On close");
    }

    void OnLeave()
    {

    }
}
