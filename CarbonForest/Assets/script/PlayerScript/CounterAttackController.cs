using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttackController : MonoBehaviour {
    private Animator animator;
    private BlockController blockController;
    private string[] actionNames = new string[] { "Counter1", "Counter2"};
    private PlayerMovement movement;

    public bool countering = false;
    public float FXOffset = 1;
    public GameObject[] counterAttackFX;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        blockController = GetComponent<BlockController>();
        movement = GetComponent<PlayerMovement>();
	}
	
    public void PlayRandomCounterAttack()
    {
        foreach(string actionName in actionNames)
        {
            animator.ResetTrigger(actionName);
        }
        StopAllCoroutines();
        countering = true;
        animator.SetTrigger(actionNames[Random.Range(0, actionNames.Length)]);
        Instantiate(counterAttackFX[Random.Range(0, 2)], 
            new Vector3(
                (movement.facingRight == true ? transform.position.x + FXOffset : transform.position.x - FXOffset),
                transform.position.y,
                transform.position.z), 
            Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0, 360f)));
        StartCoroutine(DisableCounterAfterDuation());
    }

    IEnumerator DisableCounterAfterDuation()
    {
        yield return new WaitForSeconds(0.5f);
        countering = false;
    }

    void DisableCounter()
    {
        countering = false;
    }
}
