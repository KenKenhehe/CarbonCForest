using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryHintIconObject : MonoBehaviour
{
    PlayerGeneralHandler player;
    Transform parentToFollow;
    AncientEnemyCQCTutorial enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.transform.GetComponentInParent<AncientEnemyCQCTutorial>();
        parentToFollow = transform.parent;
        print(parentToFollow);
        player = PlayerGeneralHandler.instance;
        setAllChildActive(false);
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeStatusOnPlayerBlock();
        if (parentToFollow != null)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, parentToFollow.position + new Vector3(0, 2, 0), Time.deltaTime * 10);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ChangeStatusOnPlayerBlock()
    {
        if(player.GetComponent<BlockController>().blocking == true)
        {
           
            if (enemy.GetColorState() == player.colorState)
            {
                setAllChildActive(true);
            }
            else
            {
                setAllChildActive(false);
            }
        }
        else
        {
            setAllChildActive(false);
        }
    }

    void setAllChildActive(bool active)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
