using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirection : MonoBehaviour
{
    public GameObject swordAttack;
    public Transform hitDirrection;
    public float offset;

    private float timeBtwAttacks;
    public float startTimeBtwAttacks;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwAttacks <= 0)
        {
            //hitDirrection.position += new Vector3(0.16f,0,0);
            //Instantiate(swordAttack, hitDirrection.position, transform.rotation);
            //hitDirrection.position += new Vector3(-0.16f, 0, 0);
            timeBtwAttacks = startTimeBtwAttacks;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
    }
}
