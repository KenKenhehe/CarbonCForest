using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLvFourController : EnemyCQC
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
    }

    public override void AttackPlayer()
    {
        
    }
}
