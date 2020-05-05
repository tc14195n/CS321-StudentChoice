﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIController : MonoBehaviour
{
    NavMeshAgent nav;
    public Transform target;
    public PlayerHealthController phc;
    public CharacterController cC;
    public float detectionDistance;
    public float stopDistance;
    private float distance;
    private Animator animator;
    public bool isRunner, playerSeeker;
    public float walkSpeed, runSpeed, crawlSpeed, ZombieAttackDamage, maxChaseDistance;
    ZombieHealthController zomhealth;
    public bool isStunning = false;
    public AudioClip sfx_growl, sfx_walk, sfx_attack;
    int s_len; // total number of sources
    int current_channel;
    AudioSource[] audio_channels;
    AudioSource as_growl, as_attack, as_walk;


    public enum ZombieState{
        idle, 
        walking,
        running, 
        crawling,
        attacking,
        flinch,
        crawlAttack
    }

    public ZombieState zombieState = ZombieState.idle; //default zombie to idle

    // Start is called before the first frame update
    void Start()
    {
        audio_channels = GetComponents<AudioSource>();
        as_growl = audio_channels[0];
        as_growl.clip = sfx_growl;
        as_growl.loop = false;
        as_growl.volume = 0.5f;

        as_attack = audio_channels[1];
        as_attack.clip = sfx_attack;
        as_attack.loop = false;
        as_attack.volume = 0.5f;
        
        as_walk = audio_channels[2];
        as_walk.clip = sfx_walk;
        as_walk.loop = false;
        as_walk.volume = 0.5f;

        s_len = audio_channels.Length;
        current_channel = 0;
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        zomhealth = GetComponent<ZombieHealthController>();

        if (isRunner) //check if this zombie should run
        {
            animator.SetBool("Runner", true); //just to make sure the right animations play
        }

        //StartCoroutine(Zombie());
    }
    private void FixedUpdate()
    {
        distance = Vector3.Distance(target.position, transform.position); // distance from player

        switch (zombieState)
        {
            case ZombieState.idle:
                as_walk.loop = false;
                as_growl.loop = false;
                
                //TODO: ADD IDLE SOUND HERE
                animator.SetBool("Idle", true);

                if (playerSeeker) //Make the zombie chase the player no matter where he is at
                {
                    animator.SetBool("Idle", false);

                    if (isRunner) //decide to run or walk towards the player //Suggestion: If the zombie is a walker but too far from the player it can turn into a runner to catch up than at a safe distance walk again
                    {
                        zombieState = ZombieState.running;
                    }
                    else
                    {
                        zombieState = ZombieState.walking;
                    }                   
                }

                if (zomhealth.gotShot) // check if zombie was shot in the chest to make him flinch
                {
                    animator.SetBool("Idle", false);
                    zombieState = ZombieState.flinch;
                }  

                if (zomhealth.legShot) // check if zombie was shot in the leg to make him crawl
                {
                    animator.SetBool("Idle", false);
                    zombieState = ZombieState.crawling;
                }

                if (distance <= detectionDistance && !isRunner) //check if player is in dection radius then walk
                {
                    animator.SetBool("Idle", false);
                    zombieState = ZombieState.walking;
                }

                if (distance <= detectionDistance && isRunner) //check if player is in dection radius then run
                {
                    animator.SetBool("Idle", false);
                    zombieState = ZombieState.running;
                }

                nav.SetDestination(transform.position); //stay put
                break;

            case ZombieState.walking:
                animator.SetBool("Walking", true);
                as_growl.loop = true;
                if (!as_growl.isPlaying)
                    as_growl.Play();
                as_walk.loop = true;
                if (!as_walk.isPlaying)
                    as_walk.Play();
                //TODO: ADD WALKING SOUND HERE
                

                if (distance >= maxChaseDistance && !playerSeeker) // Stop chasing the player if he has escaped and the zombie is not set to chase the player
                {
                    animator.SetBool("Walking", false);
                    zombieState = ZombieState.idle;
                }

                if (zomhealth.gotShot) // check if zombie was shot in the chest to make him flinch
                {
                    animator.SetBool("Walking", false);
                    zombieState = ZombieState.flinch;
                }
                if (zomhealth.legShot) // check if zombie was shot in the leg to make him crawl
                {
                    animator.SetBool("Walking", false);
                    zombieState = ZombieState.crawling;
                }
                if (distance <= stopDistance) // check if player is in attack radius then attack
                {
                    animator.SetBool("Walking", false);
                    nav.SetDestination(transform.position);
                    zombieState = ZombieState.attacking;
                }
                nav.speed = walkSpeed; // set the walk speed
                nav.SetDestination(target.position); // move to player
                break;

            case ZombieState.running:
                animator.SetBool("Running", true);
                //TODO: ADD RUNNING SOUND HERE

                if (distance >= maxChaseDistance && !playerSeeker) // Stop chasing the player if he has escaped and the zombie is not set to chase the player
                {
                    animator.SetBool("Running", false);
                    zombieState = ZombieState.idle;
                }

                if (zomhealth.gotShot) // check if zombie was shot in the chest to make him flinch
                {
                    animator.SetBool("Running", false);
                    zombieState = ZombieState.flinch;
                }

                if (zomhealth.legShot) // check if zombie was shot in the leg to make him crawl
                {
                    animator.SetBool("Running", false);
                    zombieState = ZombieState.crawling;
                }

                if (distance <= stopDistance + 0.25) // check if player is in attack radius then attack
                {
                    animator.SetBool("Running", false);
                    zombieState = ZombieState.attacking;
                }

                nav.speed = runSpeed; // set run speed
                nav.SetDestination(target.position); // move to player
                break;

            case ZombieState.crawling:
                as_walk.loop = false;
                animator.SetBool("Crawling", true);
                animator.SetBool("Runner", false);
                //TODO: ADD CRAWLING SOUND HERE

                if (distance >= maxChaseDistance && !playerSeeker) // Stop chasing the player if he has escaped and the zombie is not set to chase the player
                {
                    nav.SetDestination(transform.position); //stop the crawler instead of going into idle animation to avoid floating zombies
                }

                if (distance <= stopDistance - 0.5) // check if player is in attack radius then attack
                {

                    //Suggestion: add player stun
                    nav.SetDestination(transform.position);
                    zombieState = ZombieState.crawlAttack;
                }

                nav.speed = crawlSpeed; // set crawlSpeed
                nav.SetDestination(target.position); // move to player
                break;

            case ZombieState.attacking:               
                animator.SetBool("Attacking", true);
                if (!as_attack.isPlaying)
                    as_attack.Play();

                //TODO: ADD ATTACK SOUND HERE

                Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z); // used to make sure the zombie...
                transform.LookAt(targetPos);                                                                // looks at the player when attacking

                if (zomhealth.legShot) // check if zombie was shot in the leg to make him crawl
                {
                    animator.SetBool("Attacking", false);
                    zombieState = ZombieState.crawling;
                }

                if (distance > stopDistance + 0.5 && !isRunner) // start walking if zombie isnt in attack radius
                {
                    animator.SetBool("Attacking", false);
                    zombieState = ZombieState.walking;
                }

                if (distance > stopDistance + 1 && isRunner) // start running if zombie isnt in attack radius
                {
                    animator.SetBool("Attacking", false);
                    zombieState = ZombieState.running;
                }

                nav.SetDestination(transform.position); // stay put
                break;

            case ZombieState.flinch:
                nav.SetDestination(transform.position); //dont move                     
                animator.SetBool("Flinch", true);
                //TODO: ADD FLINCH SOUND HERE
                StartCoroutine(wait());
                if (isRunner) //start running again if zombie is a runner else, walk again
                {
                    animator.SetBool("Running", true);
                    zombieState = ZombieState.running;
                }
                else
                {                    
                    animator.SetBool("Walking", true);
                    zombieState = ZombieState.walking;
                }
                break;

            case ZombieState.crawlAttack:
                animator.SetBool("Attacking", true);

                targetPos = new Vector3(target.position.x, transform.position.y, target.position.z); // used to make sure the zombie...
                transform.LookAt(targetPos);                                                                // looks at the player when attacking

                if (!phc.gameObject.GetComponent<PlayerMove>().isStunned)
                {
                    phc.gameObject.GetComponent<PlayerMove>().isStunned = true;
                    isStunning = true;
                   // StartCoroutine(stun());
                }

                if (distance > stopDistance + 1) // start crawling if zombie isnt in attack radius
                {
                    animator.SetBool("Attacking", false);
                    phc.gameObject.GetComponent<PlayerMove>().isStunned = false;
                    zombieState = ZombieState.crawling;
                }

                nav.SetDestination(transform.position); // stay put
                break;

            default:
                break;
        }
    }

    IEnumerator wait()
    {
        if (zomhealth.legShot) //if shot in leg crawl
        {
            animator.SetBool("Flinch", false);
            zombieState = ZombieState.crawling;
        }
        yield return new WaitForSeconds(5f); // wait flinch duration

        if (zomhealth.legShot) //if shot in leg crawl
        {
            animator.SetBool("Flinch", false);
            zombieState = ZombieState.crawling;
        }
        zomhealth.gotShot = false; // stop flinching
        animator.SetBool("Flinch", false);
    }

    IEnumerator stun()
    {
        phc.gameObject.GetComponent<PlayerMove>().isStunned = true;
        isStunning = true;
        yield return new WaitForSeconds(1f);
        phc.gameObject.GetComponent<PlayerMove>().isStunned = false;
        isStunning = false;
        yield return new WaitForSeconds(3f);
    }

    public void damagePlayer()
    {
        phc.damage(ZombieAttackDamage); //animtion events call damagePlayer and damagePlayer sends the damage amount to the player health controller
    }
    
}
