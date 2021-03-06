﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerGeneralHandler : MonoBehaviour {
    public static PlayerGeneralHandler instance;

    public Color OriginColor;
    public Color DamagedColor;
    
    public Color blockWhite;
    public Color blockBlue;

    public int currentLevel;

    public GameObject powerUpFX;
    public GameObject parryFailFX;

    public float startHealth;
    private float healthPoints;

    public float startBlockPoint = 60;
    public float blockPoints;
    public float blockCriticalPoint = 10;
    public float blockLossRate = 10;
    public Text healthPointText;
    public Text blockPointText;

    [Header("bars...")]
    public Image healthBar;
    public Image blockBar;

    public bool canBlock = true;
    public bool AttackEnabledAfterTut = true;

    public Transform plane;

    public SpriteRenderer blockRenderer;

    private BlockController blockController;
    private SceneEventHandler sceneEventHandler;
    private Animator animator;
    private SpriteRenderer renderer;

    private ShakeController shakeController;

    private GameStateSwitch gameSwitch;
    private CounterAttackController counterAttack;
    private Animator blockAnimator;

    private Color originalColor;

    public int colorState = 0; //0: white, 1: blue
    // Use this for initialization
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        animator = GetComponent<Animator>();
    }

    void Start () {
        originalColor = GetComponent<SpriteRenderer>().color;
        shakeController = FindObjectOfType<ShakeController>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        
        renderer = GetComponent<SpriteRenderer>();
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        gameSwitch = FindObjectOfType<GameStateSwitch>();
        counterAttack = GetComponent<CounterAttackController>();
        
        blockController = GetComponent<BlockController>();
        healthPoints = startHealth;
        blockPoints = startBlockPoint;

        //--------
        //For actual game save
        //transform.position = 
        //  new Vector3(Saver.Load().position[0], Saver.Load().position[1], Saver.Load().position[2]);
        //--------
    }
	
	// Update is called once per frame
	void Update () {
        HandleBlockInput();
        HandleBlockState();
        plane.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - .75f, transform.position.z);
        HandleInteractable();
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
            if (interactable != null)
            {
                interactable.OnClose();
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.Interact();
                    print("Inrer");
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
        blockBar.fillAmount = blockPoints / startBlockPoint;
        if (blockController.blocking == false)
        {
            if (blockPoints < startBlockPoint)
            {
                blockPoints += Time.deltaTime * 10;
            }
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("Fire4") == 1) && 
            canBlock == true && blockPoints > 0)
        {
            ClearAttackState();
            if (blockController.blocking == false)
            {
                animator.SetTrigger("DefendAnimationTrigger");
                EnableBlock();
            }
            blockPoints -= Time.deltaTime * blockLossRate;
            ChangeEnemyAlphaAndBlockBar(.2f);
        }
        else if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("Fire4") < 1) && 
            blockController.blocking == true)
        {
            blockController.DisableBlocking();
            GetComponent<PlayerAttack>().enabled = true;
            ChangeEnemyAlphaAndBlockBar(0);
        }
        else
        {
            ChangeEnemyAlphaAndBlockBar(0);
            blockController.blocking = false;
        }
        if(blockPoints <= 0)
        {
            StartCoroutine(StunnedAndRecover());
        }

        if(blockController.blocking == true)
        {
            ChangeBlockColor();
            if (Input.GetKeyDown(KeyCode.J) || Input.GetButtonDown("Fire1"))
            {
                GetComponent<PlayerAttack>().attacking = false;
                colorState = 0;
            }
            else if (Input.GetKeyDown(KeyCode.K) || Input.GetButtonDown("Fire3"))
            {
                colorState = 1;
                GetComponent<PlayerAttack>().attacking = false;
            }
        }
    }

    public void TakeEnemyDamage(int damage, int EnemyColorState, Enemy enemy)
    {
        if (enemy == null || blockController.blocking == false || 
            enemy.facingRight == GetComponent<PlayerMovement>().facingRight)
        {
            healthPoints -= damage;
            healthBar.fillAmount = healthPoints / startHealth;
            if (healthPoints <= 0)
            {
                PlayerDead();
            }
            StartCoroutine(DamageEffect());
        }
        else if(blockController.blocking == true && 
            GetComponent<PlayerMovement>().facingRight != enemy.facingRight)
        {
            counterAttack.PlayRandomCounterAttack();
            if(EnemyColorState != colorState)
            {
                StartCoroutine(StunnedAndRecover());
                GetComponent<PlayerMovement>().parryStunned = true;
                healthPoints -= (damage * 2);
                blockController.DisableBlocking();
                blockController.blocking = false;
               
            }
        }
    }

    void ClearAttackState()
    {
        GetComponent<PlayerMovement>().speed = GetComponent<PlayerMovement>().walkSpeed;
        GetComponent<PlayerAttack>().attacking = false;
    }

    IEnumerator DamageEffect()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);

        yield return new WaitForSeconds(0.2f);

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
        print("RRR");
        GetComponent<PlayerAttack>().enabled = true;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = true;
        }
        PlayerMovement.Instance.SetMovement(true);
        GetComponent<BlockController>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void PlayerDead()
    {
        sceneEventHandler.gameOver = true;
        gameSwitch.ShowGameOverState(false);
        animator.SetTrigger("Dead");
        DeactivateControl();
        Destroy(gameObject, 1.5f);
    }

    IEnumerator StunnedAndRecover()
    {
        Time.timeScale = .002f;
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
        yield return new WaitForSeconds(2f);
        PlayerMovement.Instance.canMove = true;
        animator.SetBool("BlockFailIdle", false);
        canBlock = true;
        GetComponent<PlayerAttack>().enabled = true;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = true;
        }
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

    void ChangeBlockColor()
    {
        if(colorState == 0)
        {
            blockRenderer.color = blockWhite;
        }
        else if(colorState == 1)
        {
            blockRenderer.color = blockBlue;
        }
    }

    void ChangeEnemyAlphaAndBlockBar(float alpha)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
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
