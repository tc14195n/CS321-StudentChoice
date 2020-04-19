using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage;
    public float limbsDamageMultiplier, bodyDamageMultiplier, headDamageMultiplier;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    private void shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
        //    Debug.Log(hit.transform.tag);
        //    Debug.Log(hit.transform.name);
            ZombieController zombie = hit.transform.GetComponentInParent<ZombieController>();

            if (hit.transform.tag.CompareTo("Head") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieController>();
                zombie.Damage(damage, headDamageMultiplier);
            }

            else if (hit.transform.tag.CompareTo("Limbs") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieController>();
                zombie.DamageLimbs(damage, limbsDamageMultiplier, hit.transform.name);
               // Debug.Log(zombie.health);
            }

            else if (hit.transform.tag.CompareTo("Body") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieController>();
                zombie.Damage(damage, headDamageMultiplier);
            }

        }
    }
}
