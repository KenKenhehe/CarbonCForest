using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayerSorter : MonoBehaviour {

    public int layer;
    private void Awake()
    {
        GetComponent<MeshRenderer>().sortingOrder = layer;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
