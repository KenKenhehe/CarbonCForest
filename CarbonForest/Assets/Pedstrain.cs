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
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, 30);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveDir * moveSpeed * Time.deltaTime, 0, 0));
    }
}
