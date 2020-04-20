using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage;
    public float fireRate;
    public float impactForce;
    public float limbsDamageMultiplier, bodyDamageMultiplier, headDamageMultiplier;

    public int magSize = 30;
    public float reloadspeed = 1;
    public int ammoLeftInMag;
    private bool isReloading;
    public Animator animator;


    public Transform muzzle;
    public GameObject muzzleFlash;
    public GameObject enemyimpactFX, brickImpactFX, concreteImpactFX, dirtImpactFX, foliageImpactFX, glassImpactFX, metalImpactFX, plasterImpactFX, rockImpactFX, waterImpactFX, woodImpactFX;

    public Camera cam;

    private float shootCooldown;

    // Start is called before the first frame update
    void Start()
    {
        ammoLeftInMag = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (ammoLeftInMag <= 0 || (Input.GetKeyDown(KeyCode.R) && ammoLeftInMag < magSize))
        {
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= shootCooldown)
        {
            shootCooldown = Time.time + 1f / fireRate;
            shoot();
        }
    }

   IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadspeed);
        ammoLeftInMag = magSize;
        animator.SetBool("Reloading", false);
        isReloading = false;
    }

    private void shoot()
    {
        ammoLeftInMag--;
        Instantiate(muzzleFlash, muzzle);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            //    Debug.Log(hit.transform.tag);
            //    Debug.Log(hit.transform.name);
            ZombieHealthController zombie = hit.transform.GetComponentInParent<ZombieHealthController>();

            if (hit.transform.tag.CompareTo("Head") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.Damage(damage, headDamageMultiplier, "Head");
                Instantiate(enemyimpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }

            else if (hit.transform.tag.CompareTo("Body") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.Damage(damage, bodyDamageMultiplier, "Body");
                Instantiate(enemyimpactFX, hit.point, Quaternion.LookRotation(hit.normal));
                // Debug.Log(zombie.health);
            }

            else if (hit.transform.tag.CompareTo("Limbs") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.DamageLimbs(damage, limbsDamageMultiplier, hit.transform.name);
                Instantiate(enemyimpactFX, hit.point, Quaternion.LookRotation(hit.normal));
                // Debug.Log(zombie.health);
            }

            else if (hit.transform.tag.CompareTo("Body") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.Damage(damage, headDamageMultiplier);
                Instantiate(enemyimpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Enemy") == 0)
            {
                Instantiate(enemyimpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Brick") == 0)
            {
                Instantiate(brickImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Concrete") == 0)
            {
                Instantiate(concreteImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }

            else if (hit.transform.tag.CompareTo("Dirt") == 0)
            {
                Instantiate(dirtImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Foliage") == 0)
            {
                Instantiate(foliageImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Glass") == 0)
            {
                Instantiate(glassImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Metal") == 0)
            {
                Instantiate(metalImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Plaster") == 0)
            {
                Instantiate(plasterImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Rock") == 0)
            {
                Instantiate(rockImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Water") == 0)
            {
                Instantiate(waterImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Wood") == 0)
            {
                Instantiate(woodImpactFX, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            

        }


    }
}
