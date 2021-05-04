using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryTutorialHandler : MonoBehaviour
{
    PlayerGeneralHandler player;
    [SerializeField] Color Black;
    [SerializeField] Color White;
    [SerializeField] SpriteRenderer UpperState;
    [SerializeField] SpriteRenderer LowerState;
    [SerializeField] SpriteRenderer MiddleState;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerGeneralHandler.instance;
        MiddleState.color = player.colorState == 1 ? Black : White;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorOnPlayerSwitchBlock();

    }

    void ChangeColorOnPlayerSwitchBlock()
    {
        if(player.GetComponent<BlockController>().blocking == true)
        {
            MiddleState.color = player.colorState == 0 ? White : Black;
            if(player.colorState == 0)
            {
                UpperState.color = Black;
                LowerState.color = White;
            }
            if (player.colorState == 1)
            {
                LowerState.color = Black;
                UpperState.color = White;
            }
        }
    }
}
