using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    public bool canOpen;
    public float height;
    public float moveSpeed;
    Vector2 OriginPosition;
    DoorController[] doorControllers;
	// Use this for initialization
	void Start () {
        OriginPosition = transform.position;
        doorControllers = FindObjectsOfType<DoorController>();
	}
	
	// Update is called once per frame
	void Update () {
        OpenDoor();
	}

    void OpenDoor()
    {
        if (canOpen)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                new Vector3(OriginPosition.x, OriginPosition.y + height), 
                Time.deltaTime * moveSpeed);
        }
    }

    public void SetToCanOpen(string doorName)
    {
        foreach (DoorController door in doorControllers)
        {
            if (door.gameObject.name == doorName)
            {
                door.canOpen = true;
            }
        }
    }
}
