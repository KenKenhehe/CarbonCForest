using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerZero : MonoBehaviour
{
    public static bool InTutorial = false;
    public bool isSlowMotion = false;

    public GameObject arrowLauncher;
    GameObject arrowLauncherObj;

    public GameObject MoveHint;
    public GameObject DodgeHint;
    public GameObject normalAttackHint;

    public GameObject IndoorDarkener;

    private GameObject MoveHintObj;
    private GameObject DodgeHintObj;
    private GameObject normalAttackHintObj;
    private GameObject ArcherTutObj;
    private GameObject swordsManTutObj;

    public Transform dodgeHintTransform;
    public Transform normalAttackHintTransform;
    public Transform heavyAttackHintTransform;
    public Transform movementHintTransform;

    public Transform arrowLauncherTransform;
    public GameObject arrowLauncherFocus;

    public GameObject blockHint;
    public GameObject colorChangeHint;

    public GameObject teachParryWindow;
    public GameObject teachBlockWindow;

    public bool hasBlock = false;
    public bool hasSpawnBlockHint;
    public int normalAttackCount = 5;
    public int heavyAttackCount = 5;
    public GameObject archerTutorial;
    public GameObject swordsManTutorial;
    public Transform archerTutorialSpawnTransform;
    public Transform swordsmanTutorialTransform;
    public Vector3 archertextOffset;


    public static TutorialManagerZero instance;
    GameObject blockHintObj;

    bool hasAttack = false;
    bool canBlock = false;

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
        StartMovementTutorial();
        player.GetComponent<PlayerGeneralHandler>().canBlock = false;
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
        CheckSlowMotion();
        BlockParryTutorial();
        BlockRangeTutorial();

    }
    void BlockRangeTutorial()
    {
        if (InTutorial == true && hasBlock == false && hasSpawnBlockHint == false)
        {
            blockHintObj = Instantiate(blockHint, player.transform.position + archertextOffset, Quaternion.identity);
            hasSpawnBlockHint = true;
        }
        else if (InTutorial == true && hasBlock == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                InTutorial = false;
                hasBlock = true;

                blockHintObj.GetComponent<Animator>().SetTrigger("FadeOut");
            }
        }
    }

    void BlockParryTutorial()
    {
        if (inParryBlock == true)
        {
            if (player.GetComponent<BlockController>().blocking)
            {
                teachBlockWindow.SetActive(false);
                teachParryWindow.SetActive(true);
            }
            else
            {
                teachBlockWindow.SetActive(true);
                teachParryWindow.SetActive(false);
            }
        }
    }


    void CheckSlowMotion()
    {
        if (isSlowMotion == true)
        {
            if (Time.timeScale >= 0.05)
                Time.timeScale -= Time.deltaTime * 4f;
            if (hasBlock == false)
            {
                isSlowMotion = false;
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



    public void StartMovementTutorial()
    {
        MoveHintObj = Instantiate(MoveHint, movementHintTransform.position,
            Quaternion.identity, player.gameObject.transform);
    }

    //zero
    public void ArrowFall()
    {
        arrowLauncherObj = Instantiate(arrowLauncher, arrowLauncherTransform.position, Quaternion.identity);
        Destroy(MoveHintObj);
        StartCoroutine(DisableMovementControlForAwhile(player, 3));
        camera.FocusOnGameObjectForAwhile(arrowLauncherFocus, 2);
    }

    public void ToggleArrowFall(bool enable)
    {
        print(enable ? "Start Arrow fall" : "Stop arrow fall");
        arrowLauncherObj.SetActive(enable);
    }

    //first
    public void StartDodgeTutorial(Collider2D collision)
    {
        DodgeHintObj = Instantiate(DodgeHint, dodgeHintTransform.position,
            Quaternion.identity, collision.transform);
        DodgeHintObj.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        DodgeHintObj.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
    }

    //second
    public void startBlockTutorial()
    {
        player.GetComponent<PlayerGeneralHandler>().canBlock = true;
        Destroy(DodgeHintObj);
        ArcherTutObj = Instantiate(archerTutorial, archerTutorialSpawnTransform.position, Quaternion.identity);
        //StartCoroutine(FocusOnGameObjectAWhile(ArcherTutObj, 2));
        camera.FocusOnGameObjectForAwhile(ArcherTutObj, 2);
        StartCoroutine(DisableMovementControlForAwhile(player, 3));
    }

    //third
    public void StartParryTutorial()
    {
        inParryBlock = true;
        Destroy(normalAttackHintObj);
        Destroy(DodgeHintObj);
        swordsManTutObj = Instantiate(swordsManTutorial, swordsmanTutorialTransform.position, Quaternion.identity);
        //StartCoroutine(FocusOnGameObjectAWhile(swordsManTutObj, 1.5f));
        camera.FocusOnGameObjectForAwhile(swordsManTutObj, 1.5f);
        StartCoroutine(DisableMovementControlForAwhile(player, 3));
    }

    public void ResumeArrowFall()
    {
        ToggleArrowFall(true);
        RangedArrowLauncher launcher = arrowLauncherObj.GetComponent<RangedArrowLauncher>();
        launcher.ArrowRotationSpeed = 160;
        launcher.maxArrowLaunchCount = 5;
        launcher.minArrowLaunchCount = 1;
        launcher.minLaunchInterval = 1;
        launcher.maxLaunchInterval = 1.5f;

    }

    public void TriggerEndPointEvent()
    {
        camera.camDepth = 0;
    }

    public void StartNextTutorial(Collider2D collision)
    {
        if (hasAttack == false)
        {
            GameObject player = collision.gameObject;

            normalAttackHintObj =
                Instantiate(normalAttackHint,
                normalAttackHintTransform.position,
                Quaternion.identity, collision.transform);

            normalAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
            hasAttack = true;
        }
    }

    IEnumerator DisableMovementControlForAwhile(GameObject player, float second = 4f)
    {
        player.GetComponent<PlayerGeneralHandler>().DeactivateControl();
        player.GetComponent<Animator>().SetBool("isWalking", false);
        player.GetComponent<Animator>().SetBool("Defending", false);
        player.GetComponent<Animator>().SetBool("DefendWalkForward", false);
        player.GetComponent<Animator>().SetBool("DefendWalkBackward", false);
        yield return new WaitForSeconds(second);
        player.GetComponent<PlayerGeneralHandler>().ReactivateControl();
    }

    //IEnumerator FocusOnGameObjectAWhile(GameObject gameObject, float focusDuration)
    //{
    //    camera.player = gameObject;
    //    camera.ToggleCollider(false);
    //    yield return new WaitForSeconds(focusDuration);
    //    camera.player = player;
    //    yield return new WaitForSeconds(.5f);
    //    camera.ToggleCollider(true);
    //}

}
