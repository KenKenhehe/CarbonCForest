using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpenDirection
{
    LEFT,
    RIGHT
};

public class ElevatorController : Interactable
{
    public float moveSpeed;
    public bool atButtom;
    public Color glassColor;

    public GameObject TextFX;

    public Transform Up;
    public Transform buttom;


    public OpenDirection upOpenDoorDirection;
    public OpenDirection downOpenDoorDirection;

    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    Animator selectedUpOpenDoorAnimator;
    Animator selectedDownOpenDoorAnimator;
    public int AutoOpenRange = 5;
    bool isInLift = false;
    bool isAtBottom;
    bool doorClosed;

    public BoxCollider2D upGroundCollider;
    public BoxCollider2D bottomGroundCollider;

    Rigidbody2D rb2d;
    public Vector2 dir = new Vector2(0, -1);
    CameraControl cameraControl;
    Animator animator;

    Animator textFXAnimator;
    LevelEventManager levelEventManager;

    GameObject player;

    // Use this for initialization
    void Start()
    {
        isAtBottom = dir.y == 1 ? false : true;
        if (TextFX != null)
        {
            textFXAnimator = TextFX.GetComponent<Animator>();
        }
        rb2d = GetComponent<Rigidbody2D>();
        cameraControl = FindObjectOfType<CameraControl>();
        animator = GetComponent<Animator>();
        levelEventManager = FindObjectOfType<LevelEventManager>();
        player = PlayerGeneralHandler.instance.gameObject;
        selectedUpOpenDoorAnimator = upOpenDoorDirection == OpenDirection.LEFT ? leftDoorAnimator : rightDoorAnimator;
        selectedDownOpenDoorAnimator = downOpenDoorDirection == OpenDirection.LEFT ? leftDoorAnimator : rightDoorAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        EnableBehaviour();
        if (transform.position.y >= Up.position.y && isActive == false)
        {
            if (FindObjectOfType<DoorController>() != null)
                FindObjectOfType<DoorController>().SetToCanOpen("DoorOpenAfterLiftReach");
        }

        if(Vector2.Distance(player.transform.position, transform.position) < 3.5f)
        {
            //print("In elevator");
            isInLift = true;
        }
        else
        {
            isInLift = false;
        }

        if (!isInLift)
            CheckAutoOpenDoor();
    }

    private void FixedUpdate()
    {
        CheckIfInRange();
        MoveElevator();
    }

    void CheckAutoOpenDoor()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < AutoOpenRange)
        {
            if (isAtBottom)
                selectedDownOpenDoorAnimator.SetBool("IsOpen", true);
            else
                selectedUpOpenDoorAnimator.SetBool("IsOpen", true);
        }
        else
        {
            if (isAtBottom)
                selectedDownOpenDoorAnimator.SetBool("IsOpen", false);
            else
                selectedUpOpenDoorAnimator.SetBool("IsOpen", false);
        }

    }

    public void MoveElevator()
    {
        textFXAnimator.SetBool("Activate", isActive);
        animator.SetBool("Active", isActive);
        //moving down
        if (isActive && transform.position.y > buttom.position.y && dir.y < 0)
            moveDown();

        //moving up
        if (isActive && transform.position.y < Up.position.y && dir.y > 0)
            moveUp();

    }

    void moveUp()
    {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        SoundFXHandler.instance.Play("ElevatorLoop");
        if (transform.position.y >= Up.position.y)
        {
            isActive = false;
            interacted = false;
            SoundFXHandler.instance.Stop("ElevatorLoop");
            SoundFXHandler.instance.Play("ElevatorStop");

            //open door
            //selectedUpOpenDoorAnimator.SetBool("IsOpen", true);
            isAtBottom = false;
            StartCoroutine(waitAndOpenDoor());
        }
    }

    void moveDown()
    {
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        SoundFXHandler.instance.Play("ElevatorLoop");
        if (transform.position.y <= buttom.position.y)
        {
            isActive = false;
            interacted = false;
            SoundFXHandler.instance.Stop("ElevatorLoop");
            SoundFXHandler.instance.Play("ElevatorStop");

            //selectedDownOpenDoorAnimator.SetBool("IsOpen", true);
            isAtBottom = true;
            StartCoroutine(waitAndOpenDoor());
        }

    }

    IEnumerator waitAndOpenDoor()
    {
        yield return new WaitForSeconds(1);
        if(isAtBottom)
            selectedDownOpenDoorAnimator.SetBool("IsOpen", true);
        else
            selectedUpOpenDoorAnimator.SetBool("IsOpen", true);

        yield return new WaitForSeconds(1);

        player.GetComponent<PlayerGeneralHandler>().ReactivateControl();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.transform.SetParent(transform);
           // isInLift = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            collision.gameObject.transform.SetParent(null);
            //isInLift = false;
        }
    }

    public void ActivateElevator()
    {
        isActive = true;
        dir *= -1;
        
    }

    public override void Interact()
    {
        //base.Interact();
        //cameraControl.camDepth = 0;
        //cameraControl.offsetY += 0.03f;
        player.GetComponent<PlayerGeneralHandler>().DeactivateControl();

        if(levelEventManager != null)
            levelEventManager.OnElevatorInteract();

        if (isActive == false)
        {
            if (isAtBottom)
            {
                print("Close door");
                print(selectedDownOpenDoorAnimator);
                selectedDownOpenDoorAnimator.SetBool("IsOpen", false);
            }
            else
            {
                selectedUpOpenDoorAnimator.SetBool("IsOpen", false);
            }
            StartCoroutine(ActivateWhenDoorClosed());
        }
    }

    IEnumerator ActivateWhenDoorClosed()
    {
        yield return new WaitForSeconds(1);
        ActivateElevator();
        interacted = true;
        SoundFXHandler.instance.Play("ElevatorStart");
    }

    public override void OnClose()
    {
        isClose = true;
    }




}
