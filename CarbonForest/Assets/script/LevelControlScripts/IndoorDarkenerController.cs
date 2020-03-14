using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorDarkenerController : MonoBehaviour
{
    Animator animator;
    IndoorDarkenerController instance;
    public Transform entry;
    public Transform exit;
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
    }
    private void Update()
    {
        if(player.transform.position.x > entry.position.x && 
            player.transform.position.x < exit.position.x)
        {
            IsIndoor = true;
        }
        else
        {
            IsIndoor = false;
        }
        SetIsIndoor(IsIndoor);
    }
    public void SetIsIndoor(bool isIndoor)
    {
        animator.SetBool("IsInside", isIndoor);
    }
}
