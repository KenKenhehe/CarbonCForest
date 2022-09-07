using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeausemEventManager : LevelEventManager
{
    CameraControl cameraControl;

    private void Start()
    {
        cameraControl = FindObjectOfType<CameraControl>();
    }

    public override void OnElevatorInteract()
    {
        cameraControl.camDepth = 0;
        cameraControl.offsetY += 0.03f;
    }
}
