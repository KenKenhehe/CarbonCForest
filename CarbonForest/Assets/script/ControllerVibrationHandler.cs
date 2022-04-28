using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerVibrationHandler : MonoBehaviour
{
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    bool isVibrate;

    // Use this for initialization
    public static ControllerVibrationHandler instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, isVibrate ? 1 : 0, isVibrate ? 1 : 0);
    }

    public void SetVibration(float sec)
    {
        StartCoroutine(Vibrate(sec));
    }

    IEnumerator Vibrate(float sec)
    {
        isVibrate = true;
        yield return new WaitForSeconds(sec);
        isVibrate = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

}
