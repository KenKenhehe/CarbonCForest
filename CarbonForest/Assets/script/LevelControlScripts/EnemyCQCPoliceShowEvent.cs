using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCQCPoliceShowEvent : EnemyShowEvent
{
    public GameObject UAVDropper;
    public Transform UAVShowTransformLeft;
    public Transform UAVShowTransformRight;

    LevelEnemyEvent levelEnemyEvent;
    private void Start()
    {
        levelEnemyEvent = GetComponent<LevelEnemyEvent>();
        levelEnemyEvent.showEvent = EnemyShow;
    }

    public override void EnemyShow()
    {
        int randDir = Random.Range(0, 2);
        Transform showTransform =
            randDir == 0 ? UAVShowTransformLeft : UAVShowTransformRight;

        GameObject UAVObj = Instantiate(UAVDropper, showTransform.position, Quaternion.identity);
        if(UAVObj.GetComponent<FlyerHandler>() != null)
        {
            UAVObj.GetComponent<FlyerHandler>().facingRight = randDir == 0 ? true : false;
        }
    }


}
