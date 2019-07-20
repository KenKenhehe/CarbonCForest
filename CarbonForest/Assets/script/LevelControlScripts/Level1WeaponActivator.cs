using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1WeaponActivator : MonoBehaviour {
    public GameObject blockTutorialText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(FindObjectsOfType<EnemyCQC>().Length > 0 && 
            FindObjectOfType<PlayerGeneralHandler>().gameObject.transform.position.x < transform.position.x)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            collision.gameObject.GetComponent<PlayerGeneralHandler>().AttackEnabledAfterTut = true;
            collision.gameObject.GetComponent<PlayerAttack>().enabled = true;
            collision.gameObject.GetComponent<PlayerHeavyAttack>().enabled = true;
        }
    }
}
