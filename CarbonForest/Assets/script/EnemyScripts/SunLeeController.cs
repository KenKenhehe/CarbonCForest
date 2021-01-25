using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunleeController : EnemyCQC
{
    bool isOnBike = true;
    bool bikeMode = true;
    public RuntimeAnimatorController sunleeAnimator;
    public static SunleeController instance;
    SunLeeBikeController bikeController;
    [HideInInspector]
    public int fallDir = 1;

    bool falling;
    float currentFallingTime;
    public float FallTime;
    public float fallSpeed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentFallingTime = FallTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        bikeController = SunLeeBikeController.instance;
        gameObject.SetActive(false);
        Initialize();
        animator.runtimeAnimatorController = sunleeAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: if sunlee is 
        if(bikeMode == false)
        {
            //walk to bike

        }
        else
        {
            //EnableBehaviour();
        }
    }

    void Fall()
    {
        if (falling == true)
        {
            print("In fall");
            if (currentFallingTime <= 0)
            {
                falling = false;
                currentFallingTime = FallTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                print("Falling");
                currentFallingTime -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(fallSpeed * -bikeController.currentDir * Time.fixedDeltaTime, rb2d.velocity.y);
            }
        }
    }

    private void FixedUpdate()
    {
        Fall();
    }



    private void OnEnable()
    {
        if (isOnBike == true)
        {
            animator.SetTrigger("Fall");
        }
    }

    public void StopFall()
    {
        falling = false;
    }

    public void StartFall()
    {
        falling = true;
    }

}
