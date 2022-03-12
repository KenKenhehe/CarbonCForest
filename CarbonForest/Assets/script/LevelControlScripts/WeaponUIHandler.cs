using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIHandler : MonoBehaviour
{
    PlayerGeneralHandler player;
    Animator animator;
    Image currentWeaponUI;
    public static WeaponUIHandler instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponUI = GetComponent<Image>();
        animator = GetComponent<Animator>();
        player = PlayerGeneralHandler.instance;
    }


    public void ChangeToCurrentWeaponUI(Weapon weapon)
    {
        // Switch image to the current weapon's icon
        currentWeaponUI.sprite = weapon.weaponIcon;
        // Play switch animation
        animator.runtimeAnimatorController = weapon.ShowUIAnimatorController;
        animator.SetTrigger("ShowIcon");
    }
}
