using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {
    public Color blockStateColor;
    SpriteRenderer renderer;
    public bool blocking = false;
    public GameObject blockSparkFX;
    public GameObject blockSparkObj;
	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnBlock()
    {

    }

    public void EnableBlocking()
    {
        blocking = true;
    }

    public void DisableBlocking()
    {
        blocking = false;
    }

    public void TriggerBlockEffect()
    {
        blockSparkObj = Instantiate(blockSparkFX, transform.position, Quaternion.identity);
    }
}
