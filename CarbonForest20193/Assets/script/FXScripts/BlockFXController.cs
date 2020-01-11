using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFXController : MonoBehaviour {
    Animator animator;
    BlockController blockController;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        blockController = GetComponentInParent<BlockController>();
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("IsBlocking", blockController.blocking);
	}
}
