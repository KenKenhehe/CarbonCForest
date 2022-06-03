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

    void Start()
    {
        print(GetComponent<Enemy>());
        shakeController = FindObjectOfType<ShakeController>();
        tutorialManager = TutorialManagerZero.instance;
        Initialize();
        soundFXHandler.Play("Cloth" + Random.Range(1, 3));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            EnableBehaviour();
            PlayDynamicAnimation();
        }
    }

    public override void EnableBehaviour()
    {
        base.EnableBehaviour();
    }

    public override void TakeDamage(int damage)
    {   
        if(blockPoint > 0)
        {
            BlockPlayer();
            health -= 1;
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    void BlockPlayer()
    {
        animator.SetTrigger("Block" + Random.Range(1, 3).ToString());
        shakeController.CamShake();
        soundFXHandler.Play("SwordCling" + Random.Range(1,4));
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
        blocking = true;
        yield return new WaitForSeconds(1.5f);
        canMove = true;
        canAttack = true;
        blocking = false;
    }

    public override void ParriedBehaviour()
    {
        //print("Parry success! health restored");
        tutorialManager.ParrySuccess();
    }

    public override void PlayTakeDamageSound()
    {
        soundFXHandler.Play("DamageFlesh" + Random.Range(1, 3));
    }

    public override void DeathBehaviour()
    {
        DeathWithAnimation();
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }
    public override void PlayExplosionSound()
    {
        soundFXHandler.Play("Explode");
    }
}
