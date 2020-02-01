using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnvironmentController : MonoBehaviour
{
    int levelNum;
    SoundFXHandler soundFX;
    // Start is called before the first frame update
    void Start()
    {
        soundFX = SoundFXHandler.instance;
        levelNum = SceneManager.GetActiveScene().buildIndex;
        if(levelNum == 5)
        {
            soundFX.Play("AmbientRain");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
