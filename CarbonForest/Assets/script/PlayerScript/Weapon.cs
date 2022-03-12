﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : ScriptableObject
{
    public RuntimeAnimatorController animatorController;
    public RuntimeAnimatorController ShowUIAnimatorController;
    public float attack1Range = 0.4f;
    public float attack2Range = 1;
    public float attack3Range = 1.53f;

    public int attack1Damage;
    public int attack2Damage;
    public int attack3Damage;

    public GameObject slashFX;

    public GameObject attackShakeFX;

    public Sprite weaponIcon;

    public string Name;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public virtual void OverrideWeaponTriggerName(){}
    public virtual void PlayAttackAnimationOnAttackNum(Animator animator) {}

    public virtual void PlayHeavyAttackAnimationOnAttackNum(Animator animator) { }

    public virtual void resetTriggerNames(Animator animator) { }
 
}
