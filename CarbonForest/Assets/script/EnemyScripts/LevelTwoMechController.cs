using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoMechController : Enemy
{
    public GameObject dust;
    public GameObject player;
    public Transform spawnPoint;
    public float smoothness = 3;
    public Transform scrollTrasnform;

    public Transform BeamNormalShootTransform;
    public GameObject BeamNormalShootFX;
   
    public Transform BeamFinalShootTransform;
    public GameObject BeamFinalShootFX;

    public GameObject beamTarget;
    public GameObject beamExplosionFX;

    public float minShootBeamDuration;
    public float maxShootBeamDuration;
    private float shootBeamDuration;
    public static bool hasEnd = false;
    public GameObject beamTargetFollow;
    GameObject beamTargetFollowObj;
    // Use this for initialization
    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<MotoController>().gameObject;
        }
        hitDuration = new WaitForSeconds(.3f);
        shootBeamDuration = Random.Range(minShootBeamDuration, maxShootBeamDuration);
        StartCoroutine(ShootBeamsAtRandom());
        soundFXHandler = SoundFXHandler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            Mathf.Lerp(transform.position.x, player.transform.position.x - Random.Range(5, 12), Time.deltaTime * smoothness),
            transform.position.y);
        GetComponent<Animator>().SetBool("IsCannon", hasEnd);
        if(beamTargetFollowObj != null && hasEnd == true)
        {
            beamTargetFollowObj = 
                Instantiate(beamTargetFollowObj, player.transform.position, Quaternion.identity);

            beamTargetFollowObj.transform.position = 
                Vector3.Lerp(beamTargetFollowObj.transform.position, 
                player.transform.position, Time.deltaTime);
        }
    }

    public void SpawnDust()
    {
        Instantiate(dust, spawnPoint.position, Quaternion.identity);
        FindObjectOfType<ShakeController>().CamBigShake();
        soundFXHandler.Play("MechFootStep");
    }

    public override void TakeDamage(int damage)
    {
        Time.timeScale = .6f;
        FindObjectOfType<ShakeController>().CamBigShake();
        Instantiate(bloodFX, transform.position, Quaternion.identity);
        base.TakeDamage(damage);
        StartCoroutine(DamagedEffect());
        soundFXHandler.Play("BikeDamaged");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BikeEnemy>() != null)
        {
            collision.gameObject.GetComponent<BikeEnemy>().TakeDamage(100);
        }
    }

    public void TurrentShow()
    {
        soundFXHandler.Play("MechTurrentShow");
    }

    public void PlayPreparelaunch()
    {
        soundFXHandler.Play("MechPowerOn");
        //StartCoroutine(MultiplePowerOnSound());
    }

    IEnumerator MultiplePowerOnSound()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
           
        }
    }

    IEnumerator ShootBeamsAtRandom()
    {
        yield return new WaitForSeconds(shootBeamDuration);
        while (hasEnd == false)
        {
            GameObject beamNormalShootFX = 
                Instantiate(BeamNormalShootFX, BeamNormalShootTransform.position, Quaternion.Euler(0, 0, 180));
            soundFXHandler.Play("WeaponShoot");
            yield return new WaitForSeconds(Random.Range(.2f, .5f));
            
            GameObject beamTargetObj =
                Instantiate(beamTarget, player.transform.position,
                Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));

            Instantiate(beamExplosionFX, beamTargetObj.transform.position, beamTargetObj.transform.rotation);
            Destroy(beamTargetObj);
            soundFXHandler.Play("MissileExplode");
            FindObjectOfType<ShakeController>().CamShake();
            shootBeamDuration = Random.Range(minShootBeamDuration, maxShootBeamDuration);
            yield return new WaitForSeconds(shootBeamDuration);
        }
        
    }
}
