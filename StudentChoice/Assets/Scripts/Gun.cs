using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private Camera cam = null;
    private Animator animator = null;
    private PlayerMove playerMove = null;
    public AudioClip clip_shot, clip_reload, clip_empty;

    [Header("Position")]
    [SerializeField] Vector3 gunPos = new Vector3(0.03f, -0.12f, 0.03f);
    [SerializeField] Vector3 aimPos = new Vector3(-0.0665f, -0.07f, -0.09f);


    [Header("Stats")]
    [SerializeField] float damage = 10;
    [SerializeField] float RPM = 100;
    [SerializeField] int magSize = 30;
    private enum FireMode { SemiAuto, FullAuto, BurstFire }
    [SerializeField] FireMode SelectFire = FireMode.SemiAuto;

    [Space]
    [SerializeField] float limbsDamageMultiplier = 0.75f;
    [SerializeField] float bodyDamageMultiplier = 1f;
    [SerializeField] float headDamageMultiplier = 5f;

    [Space]
    [SerializeField] float reloadSpeed = 1;
    [SerializeField] float adsSpeed = 1;


    [Header("FX")]
    [SerializeField] Transform muzzle = null;
    [SerializeField] GameObject muzzleFlash = null;
    [SerializeField] GameObject[] impactFX = new GameObject[11] ;

    [SerializeField] public bool semi = true, full = false, burst = false;
    private int ammoLeftInMag;
    private bool isReloading, isShooting, isAiming = false;
    private float shootCooldown;
    private Collider limbCollider;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
        animator = GetComponent<Animator>();
        playerMove = GetComponentInParent<PlayerMove>();
        ammoLeftInMag = magSize;
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.R) && ammoLeftInMag < magSize + 1) && !isReloading) // reload when you press R and you have less ammo than the max + 1 to account for the round in the chamber
        {
            StartCoroutine(Reload());
            return;
        }


        if(ammoLeftInMag == 0)
        {
            animator.SetBool("Empty", true);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //TODO: ADD ADS SOUND HERE
            isAiming = true;
             transform.localPosition = Vector3.Lerp(aimPos, gunPos, Time.timeScale / adsSpeed * Time.deltaTime);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            //TODO: ADD unADS SOUND HERE
            isAiming = false;
            transform.localPosition = Vector3.Lerp(gunPos, aimPos, Time.timeScale / adsSpeed * Time.deltaTime);
        }

        if (playerMove.isMoving && !isShooting)
        {
            animator.SetBool("Walking", true);
        }
        else if (!isShooting)
        {
            animator.SetBool("Walking", false);
        }
        switch (SelectFire)
        {
            case FireMode.SemiAuto:
                if (Input.GetKeyDown(KeyCode.B) && full)
                {
                    SelectFire = FireMode.FullAuto;
                }
                else if (Input.GetKeyDown(KeyCode.B) && burst)
                {
                    SelectFire = FireMode.BurstFire;
                }

                else if (Input.GetButtonDown("Fire1") && ammoLeftInMag > 0 && !isShooting && !isReloading)
                {
                    StartCoroutine(ShootSemiAuto());
                }
                break;
            case FireMode.FullAuto:
                if (Input.GetKeyDown(KeyCode.B) && burst)
                {
                    SelectFire = FireMode.BurstFire;
                }
                else if (Input.GetKeyDown(KeyCode.B) && semi)
                {
                    SelectFire = FireMode.SemiAuto;
                }

                else if (Input.GetButton("Fire1") && ammoLeftInMag > 0 && !isShooting && !isReloading)
                {
                    StartCoroutine(ShootFullAuto(RPM));
                }
                break;
            case FireMode.BurstFire:
                if (Input.GetKeyDown(KeyCode.B) && semi)
                {
                    SelectFire = FireMode.SemiAuto;
                }
                else if (Input.GetKeyDown(KeyCode.B) && full)
                {
                    SelectFire = FireMode.FullAuto;
                }

                else if (Input.GetButton("Fire1") && ammoLeftInMag > 0 && !isShooting && !isReloading)
                {
                    StartCoroutine(ShootBurst(RPM));
                }
                break;
            default:
                break;
        }
    }
    

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("Reloading", true);
        animator.SetBool("Walking", false);
        //TODO: ADD RELOAD SOUND HERE
        SFXManager_player.playClip(clip_reload);
        yield return new WaitForSeconds(reloadSpeed);
        if (ammoLeftInMag > 0)
        {
            ammoLeftInMag = magSize + 1; // load one into chamber if the gun wasnt empty
        }
        else
        {
            ammoLeftInMag = magSize; // normal reload when the weapon is empty
        }
        animator.SetBool("Empty", false);
        animator.SetBool("Reloading", false);
        isReloading = false;
    }

    IEnumerator ShootSemiAuto()
    {

        isShooting = true;
        shoot();
        SFXManager_player.playClip(clip_shot);
        animator.SetBool("Shot", true);
        yield return new WaitForSeconds(0.25f);
        isShooting = false;
        animator.SetBool("Shot", false);
    }

    IEnumerator ShootFullAuto(float RPM)
    {
        isShooting = true;
        shoot();
        animator.SetBool("Shot", true);
        float time = (RPM / 60000); // convert to MS
        yield return new WaitForSeconds(time);
        isShooting = false;
        animator.SetBool("Shot", false);
    }

    IEnumerator ShootBurst(float RPM)
    {
        float time = (RPM / 60000) / 3; // convert to MS then divide by 3 for burst
        isShooting = true;
        shoot();
        animator.SetBool("Shot", true);       
        yield return new WaitForSeconds(time);
        isShooting = false;
        animator.SetBool("Shot", false);

        isShooting = true;
        shoot();
        animator.SetBool("Shot", true);
        yield return new WaitForSeconds(time);
        isShooting = false;
        animator.SetBool("Shot", false);

        isShooting = true;
        shoot();
        animator.SetBool("Shot", true);
        yield return new WaitForSeconds(time + 0.5f); //burst delay
        isShooting = false;
        animator.SetBool("Shot", false);
        

    }

    private void shoot()
    {
        if (getAmmoCount() <= 0)
            return;

        //TODO: ADD GUNSHOT SOUND HERE
        ammoLeftInMag--;
        Instantiate(muzzleFlash, muzzle);
        RaycastHit hit;
       
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            ZombieHealthController zombie;

            if (hit.transform.tag.CompareTo("Head") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.Damage(damage, headDamageMultiplier, "Head");
                Instantiate(impactFX[0], hit.point, Quaternion.LookRotation(hit.normal));
            }

            else if (hit.transform.tag.CompareTo("Body") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                zombie.Damage(damage, bodyDamageMultiplier, "Body");
                Instantiate(impactFX[0], hit.point, Quaternion.LookRotation(hit.normal));
                // Debug.Log(zombie.health);
            }

            else if (hit.transform.tag.CompareTo("Limbs") == 0)
            {
                zombie = hit.transform.GetComponentInParent<ZombieHealthController>();
                int i = zombie.DamageLimbs(damage, limbsDamageMultiplier, hit.transform.name); //returns 0 if a limb was detroyed

                if (i == 0) // this if statement disable the colliders on the missing limbs 
                {                   
                    limbCollider = hit.transform.GetComponent<Collider>(); 
                    if (hit.transform.GetChild(0).GetComponent<Collider>() != null)
                    {
                        hit.transform.GetChild(0).GetComponent<Collider>().enabled = false;

                        if (hit.transform.GetChild(0).GetChild(0).GetComponent<Collider>() != null)
                        {
                            hit.transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false; 
                        }
                    }

                    limbCollider.enabled = false;
                }
                Instantiate(impactFX[0], hit.point, Quaternion.LookRotation(hit.normal));
                // Debug.Log(zombie.health);
            }

            else if (hit.transform.tag.CompareTo("Enemy") == 0)
            {
                Instantiate(impactFX[0], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Brick") == 0)
            {
                Instantiate(impactFX[1], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Concrete") == 0)
            {
                Instantiate(impactFX[2], hit.point, Quaternion.LookRotation(hit.normal));
            }

            else if (hit.transform.tag.CompareTo("Dirt") == 0)
            {
                Instantiate(impactFX[3], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Foliage") == 0)
            {
                Instantiate(impactFX[4], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Glass") == 0)
            {
                Instantiate(impactFX[5], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Metal") == 0)
            {
                Instantiate(impactFX[6], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Plaster") == 0)
            {
                Instantiate(impactFX[7], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Rock") == 0)
            {
                Instantiate(impactFX[8], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Water") == 0)
            {
                Instantiate(impactFX[9], hit.point, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.tag.CompareTo("Wood") == 0)
            {
                Instantiate(impactFX[10], hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 500);
            }                              
        }
    }

    public int getAmmoCount()
    {
        return ammoLeftInMag;
    }
    public bool ADSing()
    {
        return isAiming;
    }
    public String getFiremode()
    {
        return SelectFire.ToString();
    }
}
