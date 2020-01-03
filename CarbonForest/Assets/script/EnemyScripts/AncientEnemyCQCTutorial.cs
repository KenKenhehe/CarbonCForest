using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientEnemyCQCTutorial : AncientEnemyCQC
{
    // Start is called before the first frame update
    TutorialManagerZero tutorialManager;
    int parryCount = 1;
    public float FXOffset = 1;
    public GameObject counterAttackFX;
    //ShakeController shakeController;
    //SoundFXHandler soundFXHandler;
    void Start()
    {
        //SoundFXHandler = FindObjectOfType<SoundFXHandler>();
        shakeController = FindObjectOfType<ShakeController>();
        tutorialManager = TutorialManagerZero.instance;
        Initialize();
        print(blockPoint);
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        PlayDynamicAnimation();
    }

    public override void EnableBehaviour()
    {
        base.EnableBehaviour();
    }

    public override void TakeDamage(int damage)
    {   
        if(blockPoint > 0)
        {
            print("blocked");
            BlockPlayer();
        }
        else
        {
            base.TakeDamage(damage);

        }
    }

    void BlockPlayer()
    {
        //canAttack = false;
        animator.SetTrigger("Block" + Random.Range(1, 3).ToString());
        shakeController.CamShake();
        soundFXHandler.Play("SwordCling1");
        FacePlayer();
        StartCoroutine(BlockForAWhileAndResumeAttack());
        Instantiate(counterAttackFX,
           new Vector3(
               (facingRight == true ? transform.position.x + FXOffset : transform.position.x - FXOffset),
               transform.position.y,
               transform.position.z),
           Quaternion.identity);

        transform.position = new Vector3(
                (facingRight ==
                true ?
                transform.position.x + (shockForce * unstableness) * 0.5f
                :
                transform.position.x - (shockForce * unstableness) * 0.5f),
                transform.position.y,
                transform.position.z);   
    }

    IEnumerator BlockForAWhileAndResumeAttack()
    {
        canAttack = false;
        canMove = false;
        yield return new WaitForSeconds(3);
        canMove = true;
        canAttack = true;
    }
}
