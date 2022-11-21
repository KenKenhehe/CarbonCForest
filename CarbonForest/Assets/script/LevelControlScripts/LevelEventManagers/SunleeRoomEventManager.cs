using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeRoomEventManager : LevelEventManager
{
    CameraControl cam;
    private void Start()
    {
        cam = FindObjectOfType<CameraControl>();
    }

    public override void OnElevatorInteract()
    {
        base.OnElevatorInteract();
        cam.maxX = 100;
    }
}
