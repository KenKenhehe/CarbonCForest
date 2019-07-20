using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoMechController : Enemy {
    public GameObject dust;
    public GameObject player;
    public Transform spawnPoint;
    public float smoothness = 3;
    public Transform scrollTrasnform;
	// Use this for initialization
	void Start () {
        if (player == null)
        {
            player = FindObjectOfType<MotoController>().gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(
            Mathf.Lerp(transform.position.x, player.transform.position.x - Random.Range(5, 12), Time.deltaTime * smoothness),
            transform.position.y);
	}

    public void SpawnDust()
    {
        Instantiate(dust, spawnPoint.position, Quaternion.identity);
        FindObjectOfType<ShakeController>().CamBigShake();
    }

    public override void TakeDamage(int damage)
    {
        Time.timeScale = .3f;
        FindObjectOfType<ShakeController>().CamBigShake();
        //GameObject bloodfX = Instantiate(bloodFX, transform);
        Instantiate(bloodFX, transform.position, Quaternion.identity);
        base.TakeDamage(damage);
        StartCoroutine(DamagedEffect());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<BikeEnemy>() != null)
        {
            collision.gameObject.GetComponent<BikeEnemy>().TakeDamage(100);
        }
    }
}
