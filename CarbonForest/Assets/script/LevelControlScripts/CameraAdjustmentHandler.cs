using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraPositionPreset
{
    public string presetName;
    public float cameraSpeed;
    public float offsetY;
    public float offsetX;
    public float camDepth;
}

public class CameraAdjustmentHandler : MonoBehaviour
{
    CameraControl cameraControl;
    public CameraPositionPreset currentPositionPreset;
    private void Start()
    {
        cameraControl = CameraControl.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            cameraControl.ChangePositionPreset(currentPositionPreset);
            Destroy(gameObject, 0.1f);
        }
    }
}
