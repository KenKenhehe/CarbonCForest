﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAndDestoryBgs : MonoBehaviour {
    public float speed;
    public float endX;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if(transform.localPosition.x <= endX)
        {
            Destroy(gameObject);
        }
	}
}
