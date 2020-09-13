using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedstrain : MonoBehaviour
{
    public bool isStardlled = false;
    [HideInInspector]
    public int moveDir;
    float moveSpeed = 2;
    Animator animator;
    public SpriteRenderer renderer;
    Transform leftBound;
    Transform rightBound;

    // Start is called before the first frame update
    void Start()
    {
        leftBound = GameObject.FindGameObjectWithTag("LeftBound").transform;
        rightBound = GameObject.FindGameObjectWithTag("RightBound").transform;
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0));
        if(transform.position.x < leftBound.transform.position.x - 20 || 
            transform.position.x > rightBound.transform.position.x + 20)
        {
            Destroy(gameObject);
        }
    }
}
