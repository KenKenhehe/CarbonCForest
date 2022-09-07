using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeLevelHandler : MonoBehaviour
{
    public static SunleeLevelHandler instance;
    public GameObject RightBound;
    public Vector3 RightBoundPosBossDead;
    public Vector3 RightBoundPosOrigin;

    CameraControl cameraMover;
    public float CamMaxPosXBossDead;
    public float CamMaxPosXOrigin;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraMover = CameraControl.instance;
    }

    public void OnBossDead()
    {
        RightBound.transform.position = RightBoundPosBossDead;
        cameraMover.maxX = CamMaxPosXBossDead;

        //TODO: Maybe enable a dialogue box's collider
    }
}
