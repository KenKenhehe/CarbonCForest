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
    GameObject currentBlockObj;
    public bool IsAncientUI;
    UIBarFX healthBarFX;
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
        healthBarFX = GetComponent<UIBarFX>();
        if (healthBarFX == null)
            healthBarFX = GetComponentInChildren<UIBarFX>();
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

        if(!IsAncientUI)
            StartCoroutine(FlashUIBG());

        currentBlockObj = blockWhite;
    }

    public void TriggerToIdleStateAnimation()
    {
        if(!IsAncientUI)
            animator.SetTrigger("ToIdleState");
        else
        {
            ToIdleState();
        }
    }

    public void TriggerToBlockStateAnimation()
    {
        if (!IsAncientUI)
            animator.SetTrigger("ToBlockState");
        else
        {
            ToBlockState();
        }
    }

    public void PlayTakeDamageAnimation()
    {
        if(!IsAncientUI)
            healthBarFX.PlayTakeDamageAnimation();
    }

    public void ToIdleState()
    {
        print("TOIDLE");
       
        inBlockState = false;
        foreach (GameObject obj in IdleUIGroup)
            obj.SetActive(true);
        foreach (GameObject obj in BlockUIGroup)
            obj.SetActive(false);
        if (!IsAncientUI)
        {
            healthStatusUI.GetComponent<Animator>().SetBool("IsBlockState", false);
        }
    }

    public void ToBlockState()
    {
        print("TOBLOCK");
       
        if (PlayerGeneralHandler.instance.GetComponent<BlockController>().blocking == true)
        {
            if (BlockState == 1)
            {
                currentBlockObj = blockBlueOrBlack;
                blockWhite.SetActive(false);
            }
            else
            {
                currentBlockObj = blockWhite;
                blockBlueOrBlack.SetActive(false);
            }
        }
        
        inBlockState = true;

        if (!IsAncientUI)
        {
            foreach (GameObject obj in BlockUIGroup)
                obj.SetActive(true);
            healthStatusUI.GetComponent<Animator>().SetBool("IsBlockState", true);
        }
        else
        {
            foreach (GameObject obj in IdleUIGroup)
                obj.SetActive(false);

            foreach (GameObject obj in BlockUIGroup)
                obj.SetActive(true);
        }
    }

    public void ToWhiteState()
    {
        blockBlueOrBlack.SetActive(false);
        blockWhite.SetActive(true);
        currentBlockObj = blockWhite;
        if (!IsAncientUI)
            animator.SetTrigger("ToWhite");
        BlockState = 0;

    }

    public void ToBlueOrBlackState()
    {
        blockBlueOrBlack.SetActive(true);
        blockWhite.SetActive(false);
        currentBlockObj = blockBlueOrBlack;
        if (!IsAncientUI)
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
        foreach (GameObject obj in IdleUIGroup)
            obj.SetActive(true);
    }

    public void ToDeathUI()
    {
        if (!IsAncientUI)
        {
            animator.SetTrigger("Death");
            healthStatusUI.GetComponent<Animator>().SetTrigger("Death");
        }
    }

    public void PlayHealthRecoverUIFX()
    {
        animator.SetTrigger("HealthRecover");
    }

    public void UpdateBlockValue(GameObject blockValueUI, float blockPoints, float maxBlockPoints)
    {
        if (!IsAncientUI)
        {
            blockValueUI.GetComponent<Text>().text =
                Mathf.FloorToInt(((blockPoints / maxBlockPoints) * 100)).ToString() + "%";
        }
        else
        {
            currentBlockObj.GetComponent<Image>().fillAmount = blockPoints / maxBlockPoints;
        }
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
