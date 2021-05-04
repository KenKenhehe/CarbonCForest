using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerZero : MonoBehaviour
{
    public static bool InTutorial = false;
    public bool InParryTutorial = false;
    public bool isSlowMotion = false;

    public GameObject arrowLauncher;
    GameObject arrowLauncherObj;

    public GameObject MoveHint;
    public GameObject DodgeHint;
    public GameObject normalAttackHint;
    public GameObject heavyAttackHint;
    public GameObject ChangeWeaponHint;
    public GameObject ParryFailHint;
    public GameObject ParryFailHintNoPause;
    public GameObject ParrySuccessHint;


    public GameObject IndoorDarkener;

    private GameObject MoveHintObj;
    private GameObject DodgeHintObj;
    private GameObject normalAttackHintObj;
    private GameObject heavyAttackHintObj;
    private GameObject ChangeWeaponHitnObj;
    private GameObject ArcherTutObj;
    private GameObject swordsManTutObj;
    private GameObject ParryFailObj;
    private GameObject ParrySuccessObj;

    public Transform dodgeHintTransform;
    public Transform normalAttackHintTransform;
    public Transform heavyAttackHintTransform;
    public Transform movementHintTransform;
    public Transform ChangeWeaponHintTransform;

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
    public Vector3 parryFailTextOffset;

    public static TutorialManagerZero instance;
    GameObject blockHintObj;

    bool hasAttack = false;
    bool canBlock = false;

    // --for parry tutorial--
    bool hasColor = false;
    bool inParryTutorial = false;
    bool inParryBlock = false;

    bool parrySuccessed = false;
    bool successShowing = false;

    bool hasReadTeachParry = true;
    bool shouldPauseWhenParryFail = true;

    int ExtendSuccessShowDuration = 0;
    CameraControl camera;
    GameObject player;

    SoundFXHandler soundFX;


    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        soundFX = SoundFXHandler.instance;
    }

    void Start()
    {
        camera = FindObjectOfType<CameraControl>();
        player = FindObjectOfType<PlayerGeneralHandler>().gameObject;
        if (teachParryWindow != null)
        {
            teachParryWindow.SetActive(false);
        }
        StartMovementTutorial();
        player.GetComponent<PlayerGeneralHandler>().canBlock = false;
        soundFX = SoundFXHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.F) && hasReadTeachParry == false)
        {
            hasReadTeachParry = true;
            Time.timeScale = 0.1f;
            teachBlockWindow.SetActive(true);
            ParryFailObj.SetActive(false);
            inParryBlock = true;
        }

        if (inParryBlock == true)
        {
            if (player.GetComponent<BlockController>().blocking &&
                parrySuccessed == false)
            {
                teachBlockWindow.SetActive(false);
                teachParryWindow.SetActive(true);
            }
            else if (player.GetComponent<PlayerGeneralHandler>().canBlock == true &&
                parrySuccessed == false)
            {
                teachBlockWindow.SetActive(true);
                teachParryWindow.SetActive(false);
            }
            else if (parrySuccessed == true)
            {
                teachBlockWindow.SetActive(false);
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
        arrowLauncherObj.SetActive(enable);
    }

    //first
    public void StartDodgeTutorial(Collider2D collision)
    {
        StartPopUpTutorial(DodgeHint, ref DodgeHintObj, dodgeHintTransform.position, collision.transform);
    }

    //second
    public void startBlockTutorial()
    {
        player.GetComponent<PlayerGeneralHandler>().canBlock = true;
        Destroy(DodgeHintObj);
        ArcherTutObj = Instantiate(archerTutorial, archerTutorialSpawnTransform.position, Quaternion.identity);
        camera.FocusOnGameObjectForAwhile(ArcherTutObj, 2);
        StartCoroutine(DisableMovementControlForAwhile(player, 3));
        FindObjectOfType<SoundFXHandler>().Play("ChiBa");
    }

    //third
    public void StartParryTutorial()
    {
        InParryTutorial = true;
        Destroy(normalAttackHintObj);
        //Destroy(DodgeHintObj);
        swordsManTutObj = Instantiate(swordsManTutorial, swordsmanTutorialTransform.position, Quaternion.identity);
        camera.FocusOnGameObjectForAwhile(swordsManTutObj, 1.5f);
        inParryBlock = true;
        StartCoroutine(DisableMovementControlForAwhile(player, 3));
    }

    public void ExitParryTutorial()
    {
        InParryTutorial = false;
        inParryBlock = false;

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
        shouldPauseWhenParryFail = false;
        soundFX.PlayFadeIn("Cello");
    }

    public void TriggerEndPointEvent()
    {
        camera.camDepth = 0;
    }

    //normal attack 
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

    public void StartHeavyAttackTutorial(Collider2D collision)
    {
        heavyAttackHintObj = Instantiate(heavyAttackHint, heavyAttackHintTransform.position,
            Quaternion.identity, collision.transform);
        heavyAttackHintObj.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        heavyAttackHintObj.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        StartCoroutine(DestoryObjAfterAWhile(heavyAttackHintObj, 10));
    }

    public void StartChangeWeaponTutorial(Collider2D collision)
    {
        StartPopUpTutorial(ChangeWeaponHint, ref ChangeWeaponHitnObj,
            ChangeWeaponHintTransform.position, collision.transform);
    }

    public void InParryFail()
    {
        if (ParryFailObj != null)
        {
            Destroy(ParryFailObj);
        }
       
        teachBlockWindow.SetActive(false);
        teachParryWindow.SetActive(false);
        if (ParrySuccessObj != null)
            Destroy(ParrySuccessObj);

        if(shouldPauseWhenParryFail == false)
        {
            StartPopUpTutorial(ParryFailHintNoPause, ref ParryFailObj,
             player.transform.position + parryFailTextOffset,
             player.transform);
        }
    }

    public void ReTryParry()
    {
        if (shouldPauseWhenParryFail == true)
        {
            hasReadTeachParry = false;
            //InParryFail();
            StartPopUpTutorial(ParryFailHint, ref ParryFailObj,
              new Vector3(Camera.main.transform.position.x,
              Camera.main.transform.position.y,
              0),
              null);
            Time.timeScale = 0;
           
            inParryBlock = false;
        }
        else
        {
            teachBlockWindow.SetActive(true);
            ParryFailObj.SetActive(false);
           
        }
    }


    public void ParrySuccess()
    {
        if(ParrySuccessObj != null)
            Destroy(ParrySuccessObj);
        teachParryWindow.SetActive(false);
        ParrySuccessObj = Instantiate(ParrySuccessHint,
            player.transform.position + parryFailTextOffset,
            Quaternion.identity, player.gameObject.transform);
        ParrySuccessObj.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        ParrySuccessObj.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        StartCoroutine(DestoryObjAfterAWhile(ParrySuccessObj, 3));
        StartCoroutine(ParrySuccessShowForAWhile());
        ExtendSuccessShowDuration += 3;
    }

    public void StartPopUpTutorial(GameObject objToInstantiate, ref GameObject objToHold, Vector3 spawnPosition, Transform transform)
    {
        objToHold = Instantiate(objToInstantiate, spawnPosition,
            Quaternion.identity, transform);
        objToHold.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        objToHold.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
        StartCoroutine(DestoryObjAfterAWhile(objToHold, 10));
    }

    public void StopTutorial()
    {
        inParryBlock = false;
        Destroy(teachBlockWindow);
        Destroy(teachParryWindow);
    }

    IEnumerator DestoryObjAfterAWhile(GameObject tutorial, float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(tutorial);
    }

    IEnumerator ParrySuccessShowForAWhile()
    {
        //if "Parry success" text is still showing, don't set parrySuccessed State to false
        successShowing = true;
        parrySuccessed = true;
        yield return new WaitForSeconds(ExtendSuccessShowDuration);
        successShowing = false;
        yield return new WaitForSeconds(3f);
        if (successShowing == false)
        {
            parrySuccessed = false;
            ExtendSuccessShowDuration = 0;
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

}
