using System.Collections;
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

    public int colorState = 0; //0: white, 1: blue
    // Use this for initialization
    void Start () {
        if(instance == null)
        {
            instance = this;
        }

        shakeController = FindObjectOfType<ShakeController>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        gameSwitch = FindObjectOfType<GameStateSwitch>();
        counterAttack = GetComponent<CounterAttackController>();
        
        blockController = GetComponent<BlockController>();
        healthPoints = startHealth;
        blockPoints = startBlockPoint;
        /*if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<PlayerHeavyAttack>().enabled = false;
            AttackEnabledAfterTut = false;
        }*/

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
            ChangeEnemyAlpha(.2f);
        }
        else if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("Fire4") < 1) && 
            blockController.blocking == true)
        {
            blockController.DisableBlocking();
            GetComponent<PlayerAttack>().enabled = true;
            ChangeEnemyAlpha(0); 
        }
        else
        {
            ChangeEnemyAlpha(0);
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
        if (blockController.blocking == false || 
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
        renderer.color = DamagedColor;

        yield return new WaitForSeconds(.2f);

        renderer.color = OriginColor;
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
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<BlockController>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void ReactivateControl()
    {
        GetComponent<PlayerAttack>().enabled = true;
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = true;
        }
        GetComponent<PlayerMovement>().enabled = true;
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
        Time.timeScale = .001f;
        FindObjectOfType<SoundFXHandler>().Play("EnemyParry");
        animator.SetTrigger("BlockFail");
        animator.SetBool("BlockFailIdle", true);
        if (GetComponent<PlayerHeavyAttack>() != null)
        {
            GetComponent<PlayerHeavyAttack>().enabled = false;
        }
        GetComponent<Rigidbody2D>().velocity *= 0;
        GetComponent<PlayerMovement>().enabled = false;
        canBlock = false;
        yield return new WaitForSeconds(4f);
        animator.SetBool("BlockFailIdle", false);
        canBlock = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
    }

    //if still in blocking tutorial, nothing can enable attack yet
    void CheckIfInTutAndEnableAttack()
    {
        if (AttackEnabledAfterTut == true)
        {
            
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

    void ChangeEnemyAlpha(float alpha)
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

    public void ShakeCamOnAni()
    {
        shakeController.CamShake();
    }

    public float GetHelthPoint()
    {
        return healthPoints;
    }
}
