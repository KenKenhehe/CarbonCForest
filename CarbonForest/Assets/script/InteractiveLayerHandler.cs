using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLayerHandler : MonoBehaviour
{
    public int layerOverlap = 22;
    public int layerOriginal = 10;
    SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            renderer.sortingOrder = layerOverlap;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            renderer.sortingOrder = layerOriginal;
        }
    }
}
