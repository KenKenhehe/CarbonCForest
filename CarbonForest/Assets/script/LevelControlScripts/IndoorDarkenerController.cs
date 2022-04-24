using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorDarkenerController : MonoBehaviour
{
    Animator animator;
    Animator buildingAnimator;
    IndoorDarkenerController instance;
    public GameObject buildingToEnter;
    public Transform entry;
    public Transform exit;
    public Transform HorizontalBoundry;
    public bool IsHorizontalBoundry = false;
    GameObject player;
    bool IsIndoor = false;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        player = FindObjectOfType<PlayerGeneralHandler>().gameObject;
        animator = GetComponent<Animator>();
        if(IsHorizontalBoundry == false)
            buildingAnimator = buildingToEnter.GetComponent<Animator>();
    }
    private void Update()
    {
        if(player != null)
            checkIndoor();
    }

    void checkIndoor()
    {
        if (IsHorizontalBoundry == false)
        {
            if (player.transform.position.x > entry.position.x &&
                player.transform.position.x < exit.position.x)
            {
                IsIndoor = true;
            }
            else
            {
                IsIndoor = false;
            }
        }
        else
        {
            if (player.transform.position.y < HorizontalBoundry.position.y)
            {
                IsIndoor = true;
            }
            else
            {
                IsIndoor = false;
            }
        }
        SetIsIndoor(IsIndoor);
    }
    public void SetIsIndoor(bool isIndoor)
    {
        animator.SetBool("IsInside", isIndoor);
        if (buildingAnimator != null)
        {
            buildingAnimator.SetBool("IsInside", isIndoor);
        }
    }
}
