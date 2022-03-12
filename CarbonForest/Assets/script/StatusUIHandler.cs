using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIHandler : MonoBehaviour
{
    Animator animator;
    public static StatusUIHandler instance;
    public int BlockState;
    PlayerGeneralHandler player;

    public GameObject[] IdleUIGroup;
    public GameObject[] BlockUIGroup;

    public GameObject healthStatusUI;
    public GameObject blockWhite;
    public GameObject blockBlueOrBlack;
    Image UIBG;
    bool inBlockState = false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        UIBG = GetComponent<Image>();
        if (PlayerGeneralHandler.instance != null)
        {
            BlockState = PlayerGeneralHandler.instance.colorState;
            player = PlayerGeneralHandler.instance;
        }
        blockWhite.SetActive(false);
        blockBlueOrBlack.SetActive(false);

        foreach (GameObject obj in BlockUIGroup)
            obj.SetActive(false);

        foreach (GameObject obj in IdleUIGroup)
            obj.SetActive(true);

        StartCoroutine(FlashUIBG());
    }

    public void TriggerToIdleStateAnimation()
    {
        animator.SetTrigger("ToIdleState");
    }

    public void TriggerToBlockStateAnimation()
    {
        animator.SetTrigger("ToBlockState");
    }


    public void ToIdleState()
    {
        foreach (GameObject obj in IdleUIGroup)
            obj.SetActive(true);
        inBlockState = false;
        
        healthStatusUI.GetComponent<Animator>().SetBool("IsBlockState", false);
    }

    public void ToBlockState()
    {
        foreach (GameObject obj in BlockUIGroup)
            obj.SetActive(true);
        if (PlayerGeneralHandler.instance.GetComponent<BlockController>().blocking == true)
        {
            if (BlockState == 1)
            {
                blockWhite.SetActive(false);
            }
            else
            {
                blockBlueOrBlack.SetActive(false);
            }
        }
        
        inBlockState = true;
        
        healthStatusUI.GetComponent<Animator>().SetBool("IsBlockState", true);
    }

    public void ToWhiteState()
    {
        blockBlueOrBlack.SetActive(false);
        blockWhite.SetActive(true);
        animator.SetTrigger("ToWhite");
        BlockState = 0;

    }

    public void ToBlueOrBlackState()
    {
        blockBlueOrBlack.SetActive(true);
        blockWhite.SetActive(false);
        animator.SetTrigger("ToBlue");
        BlockState = 1;
    }

    // Called by animation event
    public void DisableIdleUI()
    {
        foreach (GameObject obj in IdleUIGroup)
            obj.SetActive(false);
    }

    public void DisableBlockUI()
    {
        foreach (GameObject obj in BlockUIGroup)
            obj.SetActive(false);
    }

    public void ToDeathUI()
    {
        animator.SetTrigger("Death");
        healthStatusUI.GetComponent<Animator>().SetTrigger("Death");
    }

    IEnumerator FlashUIBG()
    {
        float flashReq = 0.1f;
        while (true)
        {
            if(Mathf.FloorToInt((player.blockPoints / player.startBlockPoint) * 100) < 20)
            {
                UIBG.color = Color.red;
                yield return new WaitForSeconds(flashReq);
                UIBG.color = Color.white;
                if (flashReq > 0.01f)
                    flashReq -= 0.01f;
            }

            yield return null;
        }
    }
}
