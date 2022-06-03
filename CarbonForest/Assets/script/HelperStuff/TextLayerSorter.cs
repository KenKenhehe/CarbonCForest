using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayerSorter : MonoBehaviour {

    public int layer;
    public string SortingLayerName = "Default";
    private void Awake()
    {
        GetComponent<MeshRenderer>().sortingOrder = layer;
        GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
