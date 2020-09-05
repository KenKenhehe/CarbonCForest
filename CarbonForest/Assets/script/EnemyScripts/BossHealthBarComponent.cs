using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarComponent : MonoBehaviour
{
    public Slider Bar;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void SetupForCombat()
    {
        Bar.GetComponent<Animator>().SetTrigger("Show");
        Bar.gameObject.SetActive(true);
        Bar.maxValue = enemy.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Bar.value = enemy.GetHealth();
    }
}
