using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicButtonUIHandler : MonoBehaviour
{
    public GameObject KeyboardUI;
    public GameObject ControllerUI;

    public int updateRate = 2;
    // Start is called before the first frame update
    void Start()
    {
        KeyboardUI.SetActive(false);
        ControllerUI.SetActive(false);
        print(Input.GetJoystickNames().Length);
       
        if (isControllerConnected())
        {
            ControllerUI.SetActive(true);
        }
        else
        {
            KeyboardUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)Time.time % updateRate == 0)
        {
            print(Input.GetJoystickNames()[1]);
            print(string.IsNullOrEmpty(Input.GetJoystickNames()[1]) == false);
            if (isControllerConnected())
            {
                KeyboardUI.SetActive(false);
                ControllerUI.SetActive(true);
            }
            else
            {
                ControllerUI.SetActive(false);
                KeyboardUI.SetActive(true);
            }
        }
    }

    bool isControllerConnected()
    {
        if(Input.GetJoystickNames().Length <= 0)
        {
            return false;
        }
        bool connected = false;
        foreach(string name in Input.GetJoystickNames())
        {
            if(string.IsNullOrEmpty(name) == false)
            {
                connected = true;
                break;
            }
        }
        return connected;
    }
}
