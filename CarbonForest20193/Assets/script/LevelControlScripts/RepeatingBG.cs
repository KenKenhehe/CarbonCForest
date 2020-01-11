using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBG : MonoBehaviour {
    public float scrollSpeed;
   
    public float startX;
    public float endX;

    public bool FinishSnap;
    // Use this for initialization
    void Start () {
        FinishSnap = false;
	}
	


    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * scrollSpeed * Time.fixedDeltaTime);
        if (!FinishSnap)
        {
            MoveAndSnap();
        }
    }

    void MoveAndSnap()
    {
        if(transform.localPosition.x <= endX)
        {
            Vector2 pos = new Vector2(startX, transform.position.y);
            transform.localPosition = pos;
        }
    }

    
}
