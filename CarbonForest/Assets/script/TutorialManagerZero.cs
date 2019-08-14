using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerZero : MonoBehaviour
{
    public GameObject HeavyAttackHint;
    public GameObject normalAttackHint;

    public Transform normalAttackHintTransform;
    public Transform heavyAttackHintTransform;

    bool hasAttack = false;
    bool hasBlock = false;
    bool hasColor = false;
    public int normalAttackCount = 5;
    public int heavyAttackCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.J))
        {
            
            if (normalAttackHint != null)
            {
                normalAttackHint.GetComponent<Animator>().SetTrigger("Pop");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (HeavyAttackHint != null)
            {
                HeavyAttackHint.GetComponent<Animator>().SetTrigger("Pop");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            if (hasAttack == false)
            {
                GameObject player = collision.gameObject;

                HeavyAttackHint =
                            Instantiate(HeavyAttackHint, heavyAttackHintTransform.position, Quaternion.identity, collision.transform);

                HeavyAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;

                normalAttackHint =
                    Instantiate(normalAttackHint,
                    normalAttackHintTransform.position,
                    Quaternion.identity, collision.transform);

                normalAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
                hasAttack = true;
            }
        }
    }
}
