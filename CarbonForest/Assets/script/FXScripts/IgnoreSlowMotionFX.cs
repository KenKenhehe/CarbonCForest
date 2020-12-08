using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSlowMotionFX : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale < 0.5)
        {
            animator.speed = 50;
        }
    }
}
