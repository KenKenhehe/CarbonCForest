using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThug : EnemyCQC
{
    public int rangeRespondRange;
    AttackMode currentAttackMode;
    public GameObject bomb;
    public Transform launchPoint;
    public enum AttackMode
    {
        Melee,
        Ranged
    }

    // Start is called before the first frame update
    void Start()
    {
        currentAttackMode = (AttackMode)Random.Range(0, 2);
        StartCoroutine(ChangeAttackMode());
        print("Start: " + currentAttackMode);
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        PlayDynamicAnimation();
    }

    public override void AttackPlayer()
    {
        if (canAttack == true)
        {
            if (currentAttackMode == AttackMode.Melee)
            {
                canMove = true;
                if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 1)
                {
                    animator.SetTrigger("Attack1");
                }
            }
            else
            {
                canMove = false;
                if (Vector2.Distance(transform.position, playerToFocus.transform.position) <= respondRange + 10)
                {
                    FacePlayer();
                    animator.SetTrigger("Attack2");
                }
            }
        }
    }

    public void LaunchBomb()
    {
        soundFXHandler.Play("WeaponFire1");
        Instantiate(bomb, launchPoint.position, Quaternion.identity);
        transform.position = new Vector3(
                    (facingRight ==
                    true ?
                    transform.position.x - (shockForce * unstableness * Random.Range(.5f, 1f))
                    :
                    transform.position.x + (shockForce * unstableness * Random.Range(.5f, 1f))),
                    transform.position.y,
                    transform.position.z
                    );
    }

    IEnumerator ChangeAttackMode()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 5));
            currentAttackMode = (AttackMode)Random.Range(0, 2);
            print(currentAttackMode);
        }
    }
}
