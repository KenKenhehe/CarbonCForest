using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public GameObject player;
    public GameObject mainPlayer;
    public float cameraSpeed = 5;
    public float offsetY = 0.2f;
    public float offsetX = 0.6f;
    public float maxX;
    public float minX;
    public float camDepth;

    [SerializeField] GameObject leftCollider;
    [SerializeField] GameObject rightCollider;
    public bool followTarget = true;

    public static CameraControl instance;

    // Use this for initialization
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        mainPlayer = PlayerGeneralHandler.instance.gameObject;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (followTarget == true)
        {
            AdjustCameraPosition();
        }
        OnlyMoveBetween(minX, maxX);
        
	}

    void AdjustCameraPosition()
    {
        if (player != null)
        {
            transform.position = new Vector3(
               Mathf.Lerp(transform.position.x + offsetX, player.transform.position.x + offsetX, Time.deltaTime * cameraSpeed),
               Mathf.Lerp(transform.position.y + offsetY, player.transform.position.y + offsetY, Time.deltaTime * cameraSpeed),
               Mathf.Lerp(transform.position.z, player.transform.position.z + camDepth, Time.deltaTime * cameraSpeed)
                );
        }
        else
        {
            transform.position = transform.position;
        }
    }

    void OnlyMoveBetween(float minX, float maxX)
    {
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }

    public void ToggleCollider(bool enable)
    {
        leftCollider.GetComponent<BoxCollider2D>().enabled = enable;
        rightCollider.GetComponent<BoxCollider2D>().enabled = enable;
    }

    public void FocusOnGameObjectForAwhile(GameObject gameObject, float focusDuration)
    {
        StartCoroutine(FocusOnGameObjectAWhileCoroutine(gameObject, focusDuration));
    }

    IEnumerator FocusOnGameObjectAWhileCoroutine(GameObject gameObject, float focusDuration)
    {
        player = gameObject;
        ToggleCollider(false);
        yield return new WaitForSeconds(focusDuration);
        player = mainPlayer;
        yield return new WaitForSeconds(1f);
        ToggleCollider(true);
    }
}
