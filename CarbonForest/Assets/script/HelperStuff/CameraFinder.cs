using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFinder : MonoBehaviour
{
    private new Camera camera;
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = camera;
    }


}
