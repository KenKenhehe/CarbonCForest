using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainNewWeaponHandler : MonoBehaviour
{
    public GameObject ObtainMemAnim1;
    public GameObject ObtainMemAnim2;
    public GameObject ObtainMemAnim3;

    public GameObject Flash;

    [Header("1 for spear, 2 for spear and SB")]
    [Range(1, 2)]
    public int ObtainedWeaponNum = 1;

    public GameObject weaponIcon;

    bool canChangeWeapon = false;

    Transform CameraTransform;
    // Start is called before the first frame update

    void Start()
    {
        ObtainMemAnim1.SetActive(false);
        ObtainMemAnim2.SetActive(false);
        ObtainMemAnim3.SetActive(false);
        Flash.SetActive(false);
        weaponIcon.SetActive(false);
        CameraTransform = Camera.main.transform.parent;
    }

    

    private void Update()
    {
        if(canChangeWeapon == true)
        {
            if(Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("RB"))
            {
                GetComponent<Animator>().SetTrigger("WeaponChanged");
                canChangeWeapon = false;
                GetComponent<SelfDestructor>().enabled = true;
            }
        }
    }

    IEnumerator PlayMemObtainAnim()
    {
        PlayerGeneralHandler.instance.DeactivateControl();
        Flash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Flash.SetActive(false);
        yield return new WaitForSeconds(1f);
        ObtainMemAnim1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ObtainMemAnim2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ObtainMemAnim3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        weaponIcon.SetActive(true);
        GetComponent<Animator>().SetTrigger("WeaponObtained");
        PlayerGeneralHandler.instance.ReactivateControl();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerGeneralHandler>() != null)
        {
            transform.parent = CameraTransform;
            transform.localPosition = Vector3.zero;
            StartCoroutine(PlayMemObtainAnim());
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Called by animation event
    public void AddWeaponToPlayerCurrentWeaponCount()
    {
        PlayerGeneralHandler.instance.GetComponent<PlayerAttack>().currentWeaponCount = ObtainedWeaponNum;
        GameStateHolder.instance.weaponCount = ObtainedWeaponNum;
        Saver.Save(GameStateHolder.instance);
        canChangeWeapon = true;

    }

}
