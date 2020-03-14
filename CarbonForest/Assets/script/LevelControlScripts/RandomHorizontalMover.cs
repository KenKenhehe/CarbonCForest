using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHorizontalMover : MonoBehaviour
{

    [SerializeField] float speedLeftRight = 1;
    // Start is called before the first frame update
    void Start()
    {
        print(Mathf.Sin(1 * Time.time));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mov =
            new Vector3(
                Mathf.Sin(speedLeftRight * Time.time),
                transform.position.y,
                transform.position.z);
        transform.position = new Vector3(
                Mathf.Sin(speedLeftRight * Time.time),
                transform.position.y,
                transform.position.z);
        print(mov);
    }
}
