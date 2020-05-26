using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public float interactionRange = 2;
    public Vector3 Range;
    public bool isActive;
    protected bool isClose = false;
    public GameObject OnCloseFX;
    public Vector2 onCloseScale;

    public virtual void Interact()
    {
        
    }

    void Start()
    {
        Range = transform.localScale;
    }

    public void EnableBehaviour()
    {
        if (OnCloseFX != null)
        {
            if (isClose == true)
            {
                OnCloseFX.transform.localScale =
                   Vector3.Lerp(OnCloseFX.transform.localScale, onCloseScale, Time.deltaTime * 10);
                if (OnCloseFX.transform.localScale.x >= onCloseScale.x - .2f)
                {
                    isClose = false;
                }
            }
            else
            {
                OnCloseFX.transform.localScale =
                    Vector3.Lerp(OnCloseFX.transform.localScale, Vector3.zero, Time.deltaTime * 10);
            }
        }
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
        isClose = true;
       
    }

}
