using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttack : MonoBehaviour {
    public float HeavyAttackRange = 1.5f;
    public int HeavyAttackDamage = 6;
    PlayerAttack playerAttack;
    
	// Use this for initialization
	void Start () {
        playerAttack = GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            playerAttack.attacking = true;
            playerAttack.PlayHeavyAttackAni();
        }
    }

    void HeavyAttack1()
    {
        FindObjectOfType<SoundFXHandler>().Play("SwordSwingHeavy");
        playerAttack.AttackAtRightTime(2, HeavyAttackRange, .6f);
    }

    void HeavyAttack2()
    {
        FindObjectOfType<SoundFXHandler>().Play("SwordSwingHeavy");
        playerAttack.AttackAtRightTime(HeavyAttackDamage, HeavyAttackRange, .7f);
    }

    void HeavyAttack3()
    {
        FindObjectOfType<SoundFXHandler>().Play("SwordSwingHeavy");
        playerAttack.AttackAtRightTime(HeavyAttackDamage, HeavyAttackRange, .8f);
    }
}
