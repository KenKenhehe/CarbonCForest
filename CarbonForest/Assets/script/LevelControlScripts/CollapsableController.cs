using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableController : MonoBehaviour
{
    [SerializeField] int collapsCount = 3;
    [SerializeField] GameObject[] floorBlocks;
    [SerializeField] Sprite[] floorSprites;
    [SerializeField] GameObject groundColider;
    int floorCrackIndex = 0;
    public static CollapsableController instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void TakeDamage()
    {
        collapsCount -= 1;
        print("Collapes: " + collapsCount);
        if (collapsCount > 0)
        {
            foreach (GameObject gameObject in floorBlocks)
            {
                if(floorCrackIndex > floorSprites.Length - 1)
                {
                    break;
                }
                gameObject.GetComponent<SpriteRenderer>().sprite = floorSprites[floorCrackIndex];
            }
            floorCrackIndex++;
        }
        else
        {
            foreach (GameObject gameObject in floorBlocks)
            {
                //TODO: spawn collapse for each object

                Destroy(gameObject);
            }

            groundColider.GetComponent<BoxCollider2D>().enabled = false;

            //TODO: spawn big collapse 

            Destroy(gameObject);
        }
    }
}
