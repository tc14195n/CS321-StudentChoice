using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthController : MonoBehaviour
{
    public float health;
    public GameObject head, body;
    public GameObject[] limbs;
    public GameObject ragdoll;
    public bool[] dismemberedLimbs;
    private ZombieAIController zomAI;
    public bool gotShoot = false;

    private void Start()
    {
        zomAI = GetComponent<ZombieAIController>();
    }
    public void Damage(float damage, float multiplier) //no dismemberment
    {
        health -= damage * multiplier; //apply damage + multiplier

        if (health <= 0) //death
        {
            Die();
        }
    }

    public void Damage(float damage, float multiplier, string bodyPart) //head and body 
    {
        if(head.name.CompareTo(bodyPart) == 0)
        {
            LimbHealth headhit = head.GetComponent<LimbHealth>(); //get the head's health component
            headhit.limbhealth -= damage * multiplier; //apply damage to head
            health -= damage * multiplier; // apply damage to zombie

            if (headhit.limbhealth <= 0)
            {
                dismemberedLimbs[0] = true;  // takes note of which limb was destroyed ([0] as the head on our check list is the first element)
                headhit.DismemberLimb(head);
                Destroy(head); //destroy head if health is less than zero
            }
        }
        else if (body.name.CompareTo(bodyPart) == 0)
        {
            health -= damage * multiplier; // apply damage to zombie
            gotShoot = true;
        }
        health -= damage * multiplier; //apply damage + multiplier

        if (health <= 0) //death
        {
            Die();
        }
    }

    public void DamageLimbs(float damage, float multiplier, string bodyPart) //Limbs
    {      
        
        //todo add a check to see what limb was hit and this effects zombie walk/ attack

        for (int i = 0; i < limbs.Length; i++) { //loop through all limbs
            if (limbs[i] != null && limbs[i].name.CompareTo(bodyPart) == 0) //check if this is the limb that was hit
            {
                LimbHealth limbhit = limbs[i].GetComponent<LimbHealth>(); // get the Health of the limb that was hit
                limbhit.limbhealth -= damage * multiplier;   //apply damage to limb
                health -= damage * multiplier; // apply damage to zombie


                if (limbhit.limbhealth <= 0)
                {
                    //TODO: spawn destroyed body parts
                    dismemberedLimbs[i+1] = true; // takes note of which limb was destroyed (i+1 as the head on our check list is the first element)
                    limbhit.DismemberLimb(limbs[i]);
                    if(i >= 6)
                    {
                        zomAI.zombieState = ZombieAIController.ZombieState.crawling;
                    }
                    Destroy(limbs[i]); //destroy limb if health is less than zero

                }
            }
        }

        if (health <= 0) //death
        {
            Die();
        }
    }

    private void Die()
    {
        //TODO: add replacement ragdoll that matches missing body parts
        Transform zombiePosition = transform;
        
        ragdoll = Instantiate(ragdoll, zombiePosition.position, zombiePosition.rotation);
        ragdoll.GetComponent<ZombieRagdoll>().Dismember(dismemberedLimbs);
        Destroy(gameObject);
    }
}
