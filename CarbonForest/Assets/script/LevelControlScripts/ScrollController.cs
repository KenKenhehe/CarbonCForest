using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollController : MonoBehaviour {
    public float musicTiming = 10;
    public float enemyMusicTiming = 20;
    public float mechMusicTiming;
    public GameObject flashBlack;

    bool scrolling = false;
    public GameObject DestoryBGs;
    public GameObject scrollingBGs;
    public GameObject bgMusic;

    public GameObject mech;
    private int levelIndex;
    public GameObject darkBluryImage;
    // Use this for initialization
    void Start () {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
	}
	
	// Update is called once per frame
	void Update () {
        //Scroll();
	}

    void Scroll()
    {
        if(scrolling == true)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<MotoController>() != null)
        {
            DestoryBGs.GetComponent<MovingAndDestoryBgs>().enabled = true;
            scrolling = true;
            scrollingBGs.SetActive(true);
            
            bgMusic.SetActive(true);                            
            StartCoroutine(WaitAndCameraAdjust());
            StartCoroutine(FlashBlackWhenFinish());
            //StartCoroutine(FlashBlackWhenFinishDemo());
            StartCoroutine(WaitAndSpawnEnemy());
            StartCoroutine(WaitAndSpawnMech());
            StartCoroutine(WaitAndFireFinishBeam());
        }
    }

    IEnumerator WaitAndCameraAdjust()
    {
        yield return new WaitForSeconds(musicTiming);
        FindObjectOfType<CameraControl>().camDepth = -4;
        FindObjectOfType<MotoController>().leftBound = 50;
        FindObjectOfType<MotoController>().rightBound = 75;
        Destroy(darkBluryImage);
    }

    IEnumerator FlashBlackWhenFinish()
    {
        yield return new WaitForSeconds(111);
        flashBlack.SetActive(true);
        FindObjectOfType<SoundFXHandler>().gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(levelIndex + 1);
    }

    /// <summary>
    /// For demo only
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashBlackWhenFinishDemo()
    {
        yield return new WaitForSeconds(111);
        FindObjectOfType<SoundFXHandler>().gameObject.SetActive(false);
        flashBlack.SetActive(true);
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitAndSpawnEnemy()
    {
        yield return new WaitForSeconds(enemyMusicTiming);
        StartCoroutine( FindObjectOfType<LevelEnemyEvent>().SpawnEnemyAtRandomX());
    }
    IEnumerator WaitAndSpawnMech()
    {
        yield return new WaitForSeconds(mechMusicTiming);
        Instantiate(mech);
    }
    IEnumerator WaitAndFireFinishBeam()
    {
        yield return new WaitForSeconds(mechMusicTiming + 29);
        LevelTwoMechController.hasEnd = true;
    }
}
