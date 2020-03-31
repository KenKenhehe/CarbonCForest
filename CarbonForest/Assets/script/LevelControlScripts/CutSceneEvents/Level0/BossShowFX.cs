using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShowFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slash()
    {
        Time.timeScale = 0.00005f;
        FindObjectOfType<ShakeController>().CamBigShake();
    }
}
