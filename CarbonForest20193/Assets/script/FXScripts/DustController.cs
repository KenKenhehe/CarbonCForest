using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour {
    public GameObject dustFX;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Instantiate(dustFX, transform.position, Quaternion.Euler(-90, 0, 0));
        }
    }

  
}
