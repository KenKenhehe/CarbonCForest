using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShakeController : MonoBehaviour {
    public Animator camShake;
    public float smoothness;
    WaitForSeconds shakeDuration = new WaitForSeconds(.1f);
    public static ShakeController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    public void CamShake()
    {
        camShake.SetTrigger("Shake");
    }

    public void CamBigShake()
    {
        camShake.SetTrigger("BigShake");
        ControllerVibrationHandler.instance.SetVibration(0.1f);
    }
}
