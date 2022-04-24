using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShowFX : MonoBehaviour
{
    public void Slash()
    {
        Time.timeScale = 0.00005f;
        FindObjectOfType<ShakeController>().CamBigShake();
    }
}
