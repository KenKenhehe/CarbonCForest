using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarFX : MonoBehaviour
{
    [SerializeField] float fillSpeed;
    Image image;
    [HideInInspector] public bool growing = false;
    [HideInInspector] public float updatedPercentage = 0;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GrowTo(updatedPercentage);
    }

    ///
    void GrowTo(float after)
    {
        if (growing == true)
        {
            image.fillAmount = Mathf.MoveTowards(image.fillAmount, after, Time.deltaTime * fillSpeed);
            if (image.fillAmount >= after)
                growing = false;
        }
    }

    public void playRestoreAnimation()
    {
        animator.Play("HealthRestoreSuccess");
        animator.Play("HealthTextPop");
        print("play animation");
    }
}
