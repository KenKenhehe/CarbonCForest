using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public float interactionRange = 2;
    public Vector3 Range;

    public bool isActive;
    Transform player;


    public virtual void Interact()
    {
        
    }

    void Start()
    {
        Range = transform.localScale;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Range);
    }

    public virtual void OnClose()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, Range, 0);
        foreach(Collider2D collider in colliders)
        {
            if (collider.GetComponent<PlayerGeneralHandler>() != null)
            {
                
            }
        }
    }

}
