using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public float interactionRange = 2;
    public Vector3 Range;
    public Vector3 RangeOffset;
    public bool isActive;
    protected bool isClose = false;
    public GameObject OnCloseFX;
    [SerializeField] GameObject interactHint;
    public Vector2 onCloseScale;
    [HideInInspector]
    public bool InRange = false;
    [HideInInspector]
    public bool interacted = false;

    public virtual void Interact()
    {
        interacted = !interacted;
    }

    void Start()
    {
        Range = transform.localScale;
    }

    protected void CheckIfInRange()
    {
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position + RangeOffset, interactionRange);
        foreach (Collider2D collider in interactables)
        {
            if(collider.GetComponent<PlayerGeneralHandler>() != null)
            {
                InRange = true;
                break;
            }
            else
            {
                InRange = false;
            }
        }

    }

    public virtual void EnableBehaviour()
    {
        if (interactHint != null)
        {
            if (isClose == true && interacted == false)
            {
                interactHint.SetActive(true);
                isClose = false;
            }
            else if(interacted == true)
            {
                interactHint.SetActive(false);
            }
            else
            {
                interactHint.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + RangeOffset, interactionRange);
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
        isClose = true;
    }

}
