using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryTutorialHandler : MonoBehaviour
{

    List<AncientEnemyCQCTutorial> enemiesWithParryHint;
    PlayerGeneralHandler player;
    [SerializeField] Color Black;
    [SerializeField] Color White;
    [SerializeField] SpriteRenderer UpperState;
    [SerializeField] SpriteRenderer LowerState;
    [SerializeField] GameObject parryHint;
    public static ParryTutorialHandler instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        enemiesWithParryHint = new List<AncientEnemyCQCTutorial>();
        player = PlayerGeneralHandler.instance;
        //MiddleState.color = player.colorState == 1 ? Black : White;
    }

    public void AttachParryHintToCurrentEnemy()
    {
        foreach (AncientEnemyCQCTutorial enemy in FindObjectsOfType<AncientEnemyCQCTutorial>())
        {
            GameObject parryHintobj = Instantiate(parryHint,
                enemy.transform.position + new Vector3(0, 2, 0), Quaternion.identity, enemy.transform);

            //parryHintobj.SetActive(false);
            enemiesWithParryHint.Add(enemy);
            print("Attached");
        }
    }

}
