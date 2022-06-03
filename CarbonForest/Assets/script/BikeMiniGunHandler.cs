using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMiniGunHandler : MonoBehaviour
{
    public Transform firePos1;
    public Transform firePos2;
    public GameObject bullet;
    public LineRenderer BulletTrail1;
    public LineRenderer BulletTrail2;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            animator.SetBool("Starting", true);
            SoundFXHandler.instance.Play("MiniGunStart");
        }
        if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.J))
        {
            animator.SetBool("Starting", false);
            SoundFXHandler.instance.Play("MiniGunFinish");
        }
    }

    public void FireAtPos1()
    {
        StartCoroutine(FireLine(firePos1, BulletTrail1));
    }

    public void FireAtPos2()
    {
        StartCoroutine(FireLine(firePos2, BulletTrail2));
    }


    IEnumerator FireLine(Transform firepos, LineRenderer lineRenderer)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firepos.position, Vector2.left);
        if (hitInfo)
        {
            print(hitInfo.transform.name);
            lineRenderer.SetPosition(0, firepos.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            if(hitInfo.transform.gameObject.GetComponent<Enemy>() != null)
            {
                hitInfo.transform.gameObject.GetComponent<Enemy>().TakeDamage(1);
            }
        }

        SoundFXHandler.instance.Play("MiniGunShoot");
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.01f);
        lineRenderer.enabled = false;
    }
}
