using System.Collections;
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
    public bool isRunner;
    public float walkSpeed, runSpeed, crawlSpeed, ZombieAttackDamage;
    ZombieHealthController zomhealth;

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
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        zomhealth = GetComponent<ZombieHealthController>();

        if (isRunner) //check if this zombie should run
        {
            animator.SetBool("Runner", true); //just to make sure the right animations play
        }

        //StartCoroutine(Zombie());
    }
    private void Update()
    {
        distance = Vector3.Distance(target.position, transform.position); // distance from player

        switch (zombieState)
        {
            case ZombieState.idle:
                animator.SetBool("Idle", true);

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
                animator.SetBool("Crawling", true);
                animator.SetBool("Runner", false);

                if (distance <= stopDistance) // check if player is in attack radius then attack
                {

                    //TODO: add player stun
                    nav.SetDestination(transform.position);
                    zombieState = ZombieState.crawlAttack;
                }

                nav.speed = crawlSpeed; // set crawlSpeed
                nav.SetDestination(target.position); // move to player
                break;

            case ZombieState.attacking:               
                animator.SetBool("Attacking", true);

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

                if (distance > stopDistance + 1) // start crawling if zombie isnt in attack radius
                {
                    animator.SetBool("Attacking", false);
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
            zombieState = ZombieState.crawling;
        }
        zomhealth.gotShot = false; // stop flinching
        animator.SetBool("Flinch", false);
    }
  /*  IEnumerator stun()
    {
        //TODO: stop player from moving
        yield return new WaitForSeconds(5f);
        
    }
    */
    public void damagePlayer()
    {
        phc.damage(ZombieAttackDamage); //animtion events call damagePlayer and damagePlayer sends the damage amount to the player health controller
    }
}
