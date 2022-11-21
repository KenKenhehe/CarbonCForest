using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerGeneralHandler : MonoBehaviour
{
    public static PlayerGeneralHandler instance;

    public Color OriginColor;
    public Color DamagedColor;

    public Color blockWhite;
    public Color blockBlue;

    public GameObject BlueStandFX;
    public GameObject WhiteStandFX;

    public int currentLevel;

    public GameObject powerUpFX;
    public GameObject parryFailFX;
    public GameObject takeDamageFX;

    public Vector3 TakeDamageFXOffset;

    public float startHealth;
    public int HealthToRestore;

    private float healthPoints;

    public float startBlockPoint = 60;
    public float blockPoints;
    public float blockCriticalPoint = 10;
    public float blockLossRate = 10;
    public Text healthPointText;
    public Text blockPointText;


    PlayerMovement playerMovement;

    [Header("bars...")]
    public Image healthBar;
    public Image healthBarIcon;
    public Text healthValue;

    public Image blockBar;
    public GameObject blockValue;

    public Image flowBar;
    public Text resCountText;

    public bool canBlock = true;
    public bool AttackEnabledAfterTut = true;

    public bool IsStunned = false;

    [HideInInspector]
    public bool hasCollideWithEdge = false;

    public Transform plane;

    public SpriteRenderer blockRenderer;

    //Player "Flow", Used to resurrect at the same place player die
    private int flowPoint;
    private int resurrectCount = 1;

    public int maxResCount = 2;


    private BlockController blockController;
    private GameHandler sceneEventHandler;
    private Animator animator;
    private SpriteRenderer renderer;

    private ShakeController shakeController;

    private SceneTransitionHandler gameSwitch;
    private CounterAttackController counterAttack;
    private Animator blockAnimator;

    private Color originalColor;
    private Color originalHealthBarColor;

    [HideInInspector]
    public bool isDead = false;

    public GameObject playerDeadPopupObj;
    PlayerDeadPopupController playerDeadPopup;

    public DecreaseRetryHandler decreaseRetryVFX;

    [Header("Larger the number, lower the chance")]
    [Range(1, 10)]
    public int chanceToStagger;
    /// <summary>
    /// //0: white, 1: blue/Black
    /// </summary>
    public int colorState = 0;
    // Use this for initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        originalHealthBarColor = healthBar.color;
        originalColor = GetComponent<SpriteRenderer>().color;
        shakeController = FindObjectOfType<ShakeController>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        renderer = GetComponent<SpriteRenderer>();
        sceneEventHandler = FindObjectOfType<GameHandler>();
        gameSwitch = FindObjectOfType<SceneTransitionHandler>();
        counterAttack = GetComponent<CounterAttackController>();

        blockController = GetComponent<BlockController>();
        healthPoints = startHealth;
        blockPoints = startBlockPoint;
        playerMovement = GetComponent<PlayerMovement>();

        if (healthValue != null)
            healthValue.text = healthPoints.ToString();
        //--------
        //For actual game save
        //transform.position = 
        //  new Vector3(Saver.Load().position[0], Saver.Load().position[1], Saver.Load().position[2]);
        //--------
        playerDeadPopup = playerDeadPopupObj.GetComponent<PlayerDeadPopupController>();
        playerDeadPopupObj.SetActive(false);

        flowBar.fillAmount = (((float)flowPoint) / 100);
        resCountText.text = resurrectCount.ToString();

        decreaseRetryVFX.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandleBlockInput();
        HandleBlockState();
        plane.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .85f, transform.position.z);
        HandleInteractable();
        HandlePlayerDeadInput();

    }

    public void ChangeBlockFXDir()
    {
        //PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement.facingRight == false)
        {
            WhiteStandFX.transform.localScale = new Vector2(-1, WhiteStandFX.transform.localScale.y);
            BlueStandFX.transform.localScale = new Vector2(-1, BlueStandFX.transform.localScale.y);
        }
        else
        {
            WhiteStandFX.transform.localScale = new Vector2(1, WhiteStandFX.transform.localScale.y);
            BlueStandFX.transform.localScale = new Vector2(1, BlueStandFX.transform.localScale.y);
        }
    }

    private void FixedUpdate()
    {

    }

    void HandleInteractable()
    {
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, 2);
        foreach (Collider2D collider in interactables)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null && interactable.InRange == true)
            {
                interactable.OnClose();
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.Interact();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2);
    }

    void HandleBlockState()
    {
        if (WhiteStandFX != null && BlueStandFX != null)
        {
            ChangeBlockFXDir();
        }
        if (blockPoints < 0)
        {
            blockController.blocking = false;
            blockController.DisableBlocking();
            gameObject.GetComponent<PlayerAttack>().enabled = true;
            animator.ResetTrigger("DefendAnimationTrigger");
        }
    }

    void HandleBlockInput()
    {
        animator.SetBool("Defending", blockController.blocking);
        //blockBar.fillAmount = blockPoints / startBlockPoint;
        //blockValue.text = Mathf.FloorToInt(((blockPoints / startBlockPoint) * 100)).ToString() + "%";

        //Change block value
        if (blockController.blocking == false)
        {
            if (blockPoints < startBlockPoint)
            {
                blockPoints += Time.deltaTime * 10;
            }
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("RT") == 1) &&
            canBlock == true && blockPoints > 0)
        {
            ClearAttackState();
            if (blockController.blocking == false)
            {
                StatusUIHandler.instance.TriggerToBlockStateAnimation();
                animator.SetTrigger("DefendAnimationTrigger");
                EnableBlock();
            }

            blockPoints -= Time.deltaTime * blockLossRate;
            StatusUIHandler.instance.UpdateBlockValue(blockValue, blockPoints, startBlockPoint);
            //ChangeEnemyAlphaAndBlockBar(.2f);
            ShowAllEnemyStand(true);
        }
        else if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("RT") < 1) &&
            blockController.blocking == true)
        {
            StatusUIHandler.instance.TriggerToIdleStateAnimation();
            blockController.DisableBlocking();
            GetComponent<PlayerAttack>().enabled = true;
            //ChangeEnemyAlphaAndBlockBar(0);
            ShowAllEnemyStand(false);
            if (WhiteStandFX != null && BlueStandFX != null)
            {
                WhiteStandFX.SetActive(false);
                BlueStandFX.SetActive(false);
            }

        }
        else
        {
            //ChangeEnemyAlphaAndBlockBar(0);
            ShowAllEnemyStand(false);
            blockController.blocking = false;
        }
        if (blockPoints <= 0)
        {
            StartCoroutine(StunnedAndRecover());

        }

        if (blockController.blocking == true)
        {
            ChangeBlockColor();
            if (Input.GetKeyDown(KeyCode.J) || Input.GetButtonDown("Fire1"))
            {
                GetComponent<PlayerAttack>().attacking = false;
                if (colorState == 1)
                {
                    colorState = 0;
                }
                else
                {
                    colorState = 1;
                }

            }
        }
    }

    public void decreaseFlowPoint()
    {
        flowPoint = 0;
        if (resurrectCount > 0)
            resurrectCount -= 1;
    }

    public void IncreaseFlowPoint()
    {
        flowPoint += (100 / 7);
        if (flowPoint >= 100)
        {
            flowPoint = 0;
            if (resurrectCount < maxResCount)
                resurrectCount += 1;
        }

        //TODO UI
        flowBar.fillAmount = (((float)flowPoint) / 100);
        resCountText.text = resurrectCount.ToString();
    }



    public void TakeEnemyDamage(int damage, int EnemyColorState, Enemy enemy)
    {
        if(enemy == null)
        {
            enemy = new Enemy();
            enemy.facingRight = !GetComponent<PlayerMovement>().facingRight; 
        }

        if (blockController.blocking == false || 
            GetComponent<PlayerMovement>().facingRight == enemy.facingRight)
        {
            SoundFXHandler.instance.Play("DamageSmall");
            int hitChance = Random.Range(1, chanceToStagger);
            Instantiate(takeDamageFX, transform.position + TakeDamageFXOffset, Quaternion.identity);
            healthPoints -= damage;
            UpdateHealthUI();

            StatusUIHandler.instance.PlayTakeDamageAnimation();
            if (healthPoints <= 0)
            {
                PlayerDead();
            }
            //Randomaly disable player's control when damage taken
            else if (Random.Range(0, 9) < 1 &&
                IsStunned == false &&
                GetComponent<BlockController>().blocking == false)
            {
                DeactivateControl();
                animator.SetTrigger("TakeDamage");
            }
            StartCoroutine(DamageEffect());

        }
        //else if (blockController.blocking == true &&
        //     GetComponent<PlayerMovement>().facingRight != enemy.facingRight)
        else
        {
            if (enemy == null || GetComponent<PlayerMovement>().facingRight != enemy.facingRight)
            {
                counterAttack.PlayRandomCounterAttack();
                if (EnemyColorState != colorState)
                {
                    StatusUIHandler.instance.TriggerToIdleStateAnimation();
                    StartCoroutine(StunnedAndRecover());
                    GetComponent<PlayerMovement>().parryStunned = true;
                    healthPoints -= (damage * 2);
                    blockController.DisableBlocking();
                    blockController.blocking = false;
                }
            }
        }
    }

    public void RestoreHealth()
    {
        UIBarFX healthBarFX = healthBar.GetComponent<UIBarFX>();
        if (healthPoints + HealthToRestore > startHealth)
        {
            UpdateHealthUI();
        }
        else
        {
            healthPoints += HealthToRestore;
            healthBarFX.playRestoreAnimation();

        }
        healthBarFX.growing = true;
        healthBarFX.updatedPercentage = healthPoints / startHealth;
        if (healthValue != null)
            healthValue.text = healthPoints.ToString();
    }

    void UpdateHealthUI()
    {
        healthBar.fillAmount = healthPoints / startHealth;
        if (healthValue != null)
            healthValue.text = healthPoints.ToString();
    }

    void ClearAttackState()
    {
        GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().walkSpeed;
        GetComponent<PlayerAttack>().attacking = false;
    }

    IEnumerator DamageEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);
        healthBar.color = Color.white;
        if (healthBarIcon != null)
            healthBarIcon.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        healthBar.color = originalHealthBarColor;
        if (healthBarIcon != null)
            healthBarIcon.color = Color.white;
        GetComponentInChildren<SpriteRenderer>().color = originalColor;
    }

    void EnableBlock()
    {
        blockController.EnableBlocking();
        gameObject.GetComponent<PlayerAttack>().enabled = false;
    }

    public void DeactivateControl()
    {
        GetComponent<PlayerAttack>().enabled = false;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = false;
        }
        PlayerMovement.Instance.SetMovement(false);
        GetComponent<BlockController>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetBool("isWalking", false);
    }

    public void ReactivateControl()
    {
        GetComponent<PlayerAttack>().enabled = true;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = true;
        }
        PlayerMovement.Instance.SetMovement(true);
        GetComponent<BlockController>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void HandlePlayerDeadInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && isDead)
        {
            if (resurrectCount <= 0)
            {
                SceneTransitionHandler.instance.restartCurrentScene();
            }
            else
            {
                StartCoroutine(resurrectInAWhile());

                //TODO Add resurrect VFX
            }
        }
    }

    IEnumerator resurrectInAWhile()
    {
        isDead = false;
        decreaseRetryVFX.gameObject.SetActive(false);
        playerDeadPopupObj.GetComponent<Animator>().SetTrigger("KeepTrySelected");
        yield return new WaitForSeconds(1f);
        StatusUIHandler.instance.gameObject.SetActive(false);
        animator.SetTrigger("Idle");
        healthPoints = startHealth;
        playerDeadPopupObj.SetActive(false);
        UpdateHealthUI();
        decreaseRetryVFX.gameObject.SetActive(true);
        decreaseRetryVFX.SetRetry(resurrectCount);
        resurrectCount -= 1;
        yield return new WaitForSeconds(1);
        flowBar.fillAmount = (((float)flowPoint) / 100);
        resCountText.text = resurrectCount.ToString();
        StatusUIHandler.instance.gameObject.SetActive(true);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ReactivateControl();
    }

    public void TopUpStatus()
    {
        healthPoints = startHealth;
        if (resurrectCount <= maxResCount)
        {
            resurrectCount += 1;
        }
        UpdateHealthUI();
        flowPoint = 0;
        flowBar.fillAmount = (((float)flowPoint) / 100);
        resCountText.text = resurrectCount.ToString();
        StatusUIHandler.instance.PlayHealthRecoverUIFX();
    }

    public void ParrySuccessFX()
    {
        SoundFXHandler.instance.Play("ParrySuccess1");
        ShakeController.instance.CamBigShake();
        Time.timeScale = 0.002f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ColliderRight")
        {
            print("COLLIDER RIGHT");
            hasCollideWithEdge = true;
        }

        if (collision.gameObject.tag == "ColliderLeft")
        {
            print("COLLIDER LEFT");
            hasCollideWithEdge = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ColliderRight")
        {
            print("COLLIDER RIGHT EXIT");
            hasCollideWithEdge = false;
        }

        if (collision.gameObject.tag == "ColliderLeft")
        {
            print("COLLIDER LEFT EXIT");
            hasCollideWithEdge = false;
        }
    }

    void PlayerDead()
    {
        isDead = true;
        playerDeadPopup.gameObject.SetActive(true);
        Time.timeScale = 0.05f;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        if (resurrectCount == 0)
        {
            playerDeadPopup.ToDeadPopup(true);
            animator.SetTrigger("Dead");
            DeactivateControl();
        }
        else
        {
            playerDeadPopup.ToDeadPopup(false);
            animator.SetTrigger("BlockFail");
            animator.SetBool("BlockFailIdle", true);
            DeactivateControl();
        }
    }

    IEnumerator StunnedAndRecover()
    {
        IsStunned = true;
        Time.timeScale = .002f;
        if (WhiteStandFX != null && BlueStandFX != null)
        {
            WhiteStandFX.SetActive(false);
            BlueStandFX.SetActive(false);
        }
        float FXRotation = GetComponent<PlayerMovement>().facingRight ? 90 : -90;
        Instantiate(parryFailFX, transform.position, Quaternion.Euler(0, 0, FXRotation));
        SoundFXHandler.instance.Play("EnemyParry");
        animator.SetTrigger("BlockFail");
        animator.SetBool("BlockFailIdle", true);
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = false;
        }
        GetComponent<Rigidbody2D>().velocity *= 0;
        PlayerMovement.Instance.canMove = false;
        canBlock = false;
        if (TutorialManagerZero.instance != null &&
            TutorialManagerZero.instance.InParryTutorial == true)
        {
            TutorialManagerZero.instance.InParryFail();
        }
        blockPoints = startBlockPoint;
        yield return new WaitForSeconds(2f);

        PlayerMovement.Instance.canMove = true;
        animator.SetBool("BlockFailIdle", false);
        canBlock = true;
        counterAttack.countering = false;
        GetComponent<PlayerAttack>().enabled = true;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = true;
        }
        if (TutorialManagerZero.instance != null &&
            TutorialManagerZero.instance.InParryTutorial == true)
        {
            TutorialManagerZero.instance.ReTryParry();
        }
        IsStunned = false;
    }

    public void CallPowerUp()
    {
        StartCoroutine(PowerUp(4f));
    }

    public IEnumerator PowerUp(float duration)
    {
        Instantiate(powerUpFX, transform);
        GetComponent<PlayerAttack>().attack3Damage *= 2;
        yield return new WaitForSeconds(duration);
        GetComponent<PlayerAttack>().attack3Damage /= 2;
    }

    void ShowAllEnemyStand(bool show)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.ShowEnemyCurrentStand(show);
        }
    }

    void ChangeBlockColor()
    {
        if (BlueStandFX != null && WhiteStandFX != null)
        {
            if (colorState == 0)
            {
                BlueStandFX.SetActive(false);
                WhiteStandFX.SetActive(true);
                StatusUIHandler.instance.ToWhiteState();
            }
            else if (colorState == 1)
            {
                BlueStandFX.SetActive(true);
                WhiteStandFX.SetActive(false);
                StatusUIHandler.instance.ToBlueOrBlackState();
            }
        }
        else
        {
            if (colorState == 0)
            {
                blockRenderer.color = blockWhite;
            }
            else if (colorState == 1)
            {
                blockRenderer.color = blockBlue;
            }
        }
    }

    public void DisplayTip(GameObject tipToDisplay, Vector2 tipOffset)
    {
        tipToDisplay.transform.parent = this.gameObject.transform;
        tipToDisplay.transform.localPosition =
            new Vector3(tipOffset.x, tipOffset.y, tipToDisplay.transform.position.z);
        StartCoroutine(tipFlashOnPlayerTop(tipToDisplay));
    }

    IEnumerator tipFlashOnPlayerTop(GameObject tipToDisplay)
    {
        yield return new WaitForSeconds(0.05f);
        tipToDisplay.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        tipToDisplay.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        tipToDisplay.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        tipToDisplay.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        tipToDisplay.SetActive(true);
        yield return new WaitForSeconds(1f);
        tipToDisplay.SetActive(false);
    }

    void ChangeEnemyAlphaAndBlockBar(float alpha)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        //MissileBehaviour[] missiles = FindObjectsOfType<MissileBehaviour>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.blockColorRenderer != null)
            {
                enemy.blockColorRenderer.color = new Color(enemy.blockColorRenderer.color.r,
                enemy.blockColorRenderer.color.g,
                enemy.blockColorRenderer.color.b,
                alpha);
            }
        }
    }

    public void SetToDefaultState()
    {
    }

    public void ShakeCamOnAni()
    {
        shakeController.CamShake();
    }

    public float GetHelthPoint()
    {
        return healthPoints;
    }
}
