using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerZero : MonoBehaviour
{
    public static bool InTutorial = false;
    public bool isSlowMotion = false;

    public GameObject HeavyAttackHint;
    public GameObject normalAttackHint;

    public Transform normalAttackHintTransform;
    public Transform heavyAttackHintTransform;

    public GameObject blockHint;
    public GameObject dodgueHint;
    public GameObject colorChangeHint;

    public GameObject teachParryWindow;
    public bool hasBlock = false;
    public bool hasSpawnBlockHint;
    public int normalAttackCount = 5;
    public int heavyAttackCount = 5;
    public GameObject archerTutorial;
    public Transform archerTutorialSpawnTransform;

    public static TutorialManagerZero instance;

    GameObject blockHintObj;

    bool hasAttack = false;

    // --for parry tutorial--
    bool hasColor = false;
    bool inParryTutorial = false;
    bool inParryBlock = false;
    CameraControl camera;
    GameObject player;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        camera = FindObjectOfType<CameraControl>();
        player = FindObjectOfType<PlayerGeneralHandler>().gameObject;
        teachParryWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            if (normalAttackHint != null)
            {
                normalAttackHint.GetComponent<Animator>().SetTrigger("Pop");
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (HeavyAttackHint != null)
            {
                HeavyAttackHint.GetComponent<Animator>().SetTrigger("Pop");
            }
        }
        */

        if (isSlowMotion == true)
        {
            if (Time.timeScale >= 0.05)
                Time.timeScale -= Time.deltaTime * 4f;
            if (hasBlock == false)
            {
                isSlowMotion = false;
            }
        }

        if (InTutorial == true && hasBlock == false && hasSpawnBlockHint == false)
        {
            blockHintObj = Instantiate(blockHint, player.transform.position, Quaternion.identity);
            hasSpawnBlockHint = true;
        }
        else if(InTutorial == true && hasBlock == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                InTutorial = false;
                hasBlock = true;
                
                blockHintObj.GetComponent<Animator>().SetTrigger("FadeOut");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            StartNextTutorial(collision);
        }
    }

    IEnumerator focusOnArcherAWhile()
    {
        camera.player = archerTutorial;
        yield return new WaitForSeconds(2f);
        camera.player = FindObjectOfType<PlayerGeneralHandler>().gameObject;
    }

    public void startBlockTutorial()
    {
        Instantiate(archerTutorial, archerTutorialSpawnTransform.position, Quaternion.identity);
        StartCoroutine(focusOnArcherAWhile()); 
    }

    public void StartParryTutorial()
    {
        if (inParryTutorial == true)
        {

        }
    }

    public void StartNextTutorial(Collider2D collision)
    {
        if (hasAttack == false)
        {
            GameObject player = collision.gameObject;

            HeavyAttackHint =
                        Instantiate(HeavyAttackHint, heavyAttackHintTransform.position, Quaternion.identity, collision.transform);

            HeavyAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;

            normalAttackHint =
                Instantiate(normalAttackHint,
                normalAttackHintTransform.position,
                Quaternion.identity, collision.transform);

            normalAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
            hasAttack = true;
        }
        else
        {

        }
    }
}
