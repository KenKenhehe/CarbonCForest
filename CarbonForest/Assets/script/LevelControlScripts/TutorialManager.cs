using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public GameObject blockHint;
    public GameObject colorHint;
    public GameObject HeavyAttackHint;
    public GameObject normalAttackHint;
    public GameObject interactionHint;

    public bool hasBlock = false;
    public bool hasSwitchColor = false;
    public bool hasAttack = false;
    public bool hasInteracted = false;

    PlayerGeneralHandler player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerGeneralHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		if(hasBlock == true && hasSwitchColor == false && 
            (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Fire4") == 1))
        {
            blockHint.GetComponent<Animator>().SetTrigger("FadeOut");
            Destroy(blockHint, 1);
            colorHint = 
                Instantiate(colorHint, player.transform.position, Quaternion.identity, player.transform);
            colorHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
            hasSwitchColor = true;
        }

        if(hasSwitchColor == true && 
            Input.GetKeyDown(KeyCode.K) && 
            Input.GetKey(KeyCode.Space) &&
            colorHint != null)
        {
            colorHint.GetComponent<Animator>().SetTrigger("FadeOut");
            Destroy(colorHint, 1);
        }
        if(hasAttack == true && Input.GetKeyDown(KeyCode.K) && HeavyAttackHint != null)
        {
            HeavyAttackHint.GetComponent<Animator>().SetTrigger("FadeOut");
            Destroy(HeavyAttackHint, 1);
        }
        if(hasAttack == true && Input.GetKeyDown(KeyCode.J) && normalAttackHint != null)
        {
            normalAttackHint.GetComponent<Animator>().SetTrigger("FadeOut");
            Destroy(normalAttackHint, 1);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            GetComponent<BoxCollider2D>().size = Vector2.zero;
            if (hasBlock == false)
            {
                blockHint = 
                    Instantiate(blockHint, collision.gameObject.transform.position, Quaternion.identity, collision.transform);
                hasBlock = true;
                blockHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
            }

            else if(hasBlock == true && hasAttack == false)
            {
                HeavyAttackHint =
                    Instantiate(HeavyAttackHint, collision.gameObject.transform.position, Quaternion.identity, collision.transform);
                
                HeavyAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;

                normalAttackHint =
                    Instantiate(normalAttackHint, 
                    (Vector2)collision.transform.position + Vector2.up * .3f, 
                    Quaternion.identity, collision.transform);

                normalAttackHint.gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
                hasAttack = true;
            }
        }
    }

    void PopOutTutorialText(GameObject hint)
    {
        GameObject hintToPop = Instantiate(hint, player.transform.position, Quaternion.identity, player.transform);
        hintToPop.GetComponentInChildren<MeshRenderer>().sortingOrder = 20;
    }
}
