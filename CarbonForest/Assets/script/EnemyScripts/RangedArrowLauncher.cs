using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedArrowLauncher : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] bool canShoot = true;
    public int minArrowLaunchCount = 5;
    public int maxArrowLaunchCount = 7;

    public float minLaunchInterval = 0.5f;
    public float maxLaunchInterval = 1;

    public float minLaunchRate = .1f;
    public float maxLaunchRate = .3f;
    bool FirstTimeEnable = true;

    [SerializeField] Vector3 spawnOffset;
    public float ArrowRotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootWithRandomInterval());
        FirstTimeEnable = false;
        ArrowRotationSpeed = 70;
    }

    private void OnEnable()
    {
        if (FirstTimeEnable == false)
        {
            StartCoroutine(ShootWithRandomInterval());
        }
    }

    IEnumerator ShootWithRandomInterval()
    {
        yield return new WaitForSeconds(1);
        while (canShoot)
        {
            for (int i = 0; i < Random.Range(minArrowLaunchCount, maxArrowLaunchCount); i++)
            {
                GameObject arrowObj = Instantiate(arrow,
                    transform.position + spawnOffset + new Vector3(Random.Range(1f, 4f), 0, 0),
                    Quaternion.Euler(0, 0, Random.Range(0f, 10f)));
                arrowObj.GetComponent<RangedArrowBehaviour>().rotateSpeed = ArrowRotationSpeed;
                yield return new WaitForSeconds(.2f);
            }

            yield return new WaitForSeconds(Random.Range(minLaunchInterval, maxLaunchInterval));
        }
    }


}
