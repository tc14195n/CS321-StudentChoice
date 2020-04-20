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
    public bool gotShot = false;
    public bool legShot = false;

    private void Start()
    {
        zomAI = GetComponent<ZombieAIController>();
    }

    public void Damage(float damage, float multiplier) //no dismemberment ignore (just for backup)
    {
        health -= damage * multiplier; //apply damage + multiplier

        if (health <= 0) //death
        {
            Die();
        }
    }

    public void Damage(float damage, float multiplier, string bodyPart) //head and body 
    {
        if(bodyPart.CompareTo("Head") == 0)
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
        else if (bodyPart.CompareTo("Body") == 0)
        {
            health -= damage * multiplier; // apply damage to zombie
            gotShot = true; // used to make the zombie flinch in the zombieAIController script
        }

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
                        legShot = true; // used to make the zombie crawl in the zombieAIController script
                        //zomAI.zombieState = ZombieAIController.ZombieState.crawling; // make the zombie crawl when missing a leg (all leg parts are <=6 in the index)
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
        //TODO: add death noise
        //Suggestion: add blood puddle
        Transform zombiePosition = transform; //get the zombies position     
        ragdoll = Instantiate(ragdoll, zombiePosition.position, zombiePosition.rotation); //spawn ragdoll at the zombies position
        ragdoll.GetComponent<ZombieRagdoll>().Dismember(dismemberedLimbs); // remove the ragdolls limbs to match the dead zombie
        Destroy(gameObject); //destroy the zombie (not the ragdoll)
    }
}
