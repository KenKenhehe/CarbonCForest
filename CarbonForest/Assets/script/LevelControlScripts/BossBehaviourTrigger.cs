using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If boss is waiting for player's apperences instead of spawning from somewhere else, 
/// use this script
/// </summary>
public class BossBehaviourTrigger : MonoBehaviour
{
    public Dialog dialog;
    [SerializeField] bool dialogueComplete = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerGeneralHandler>() != null)
        {
            if(FindObjectOfType<LevelThreeBossController>() != null)
            {
                FindObjectOfType<DialogHandler>().startDialogue(dialog);
                dialogueComplete = true;
                if (dialogueComplete == true)
                {
                    FindObjectOfType<LevelThreeBossController>().GetComponent<BossHealthBarComponent>().SetupForCombat();
                    FindObjectOfType<LevelThreeBossController>().inCombat = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
