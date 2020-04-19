using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAIController : MonoBehaviour
{
    NavMeshAgent nav;
    public Transform target;
    public float detectionDistance;
    public float stopDistance;
    private float distance;
    private Animator animator;
    public bool isRunner;
    ZombieHealthController zomhealth;

    public enum ZombieState{
        idle, 
        walking,
        running, 
        crawling,
        attacking,
        flinch
    }

    public ZombieState zombieState = ZombieState.idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        zomhealth = GetComponent<ZombieHealthController>();

        //StartCoroutine(Zombie());
    }
    private void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);
        switch (zombieState)
        {
            case ZombieState.idle:
                animator.SetBool("Idle", true);
                if (distance <= detectionDistance)
                {
                    animator.SetBool("Idle", false);
                    zombieState = ZombieState.walking;
                }
                nav.SetDestination(transform.position);
                break;

            case ZombieState.walking:
                animator.SetBool("Walking", true);
                
                if (zomhealth.gotShoot)
                {
                    zombieState = ZombieState.flinch;
                }
                if (distance <= stopDistance)
                {
                    animator.SetBool("Walking", false);
                    nav.SetDestination(transform.position);
                    zombieState = ZombieState.attacking;
                }
                nav.SetDestination(target.position);
                break;

            case ZombieState.running:
                animator.SetBool("Running", true);
                if (distance <= stopDistance)
                {
                    animator.SetBool("Running", false);
                    zombieState = ZombieState.attacking;
                }
                break;

            case ZombieState.crawling:
                animator.SetBool("Crawling", true);
                nav.SetDestination(target.position);
                if (distance <= stopDistance)
                {
                    animator.SetBool("Crawling", false);
                    zombieState = ZombieState.attacking;
                }
                break;

            case ZombieState.attacking:
                animator.SetBool("Attacking", true);
                if (distance > stopDistance)
                {
                    animator.SetBool("Attacking", false);
                    zombieState = ZombieState.walking;
                }
                break;

            case ZombieState.flinch:
                nav.SetDestination(transform.position);
                Debug.Log("gg");
                animator.SetBool("Walking", false);
                animator.SetBool("Flinch", true);

                StartCoroutine(wait());

                if (isRunner)
                {
                    zombieState = ZombieState.running;
                }
                else
                {                    
                    animator.SetBool("Walking", true);
                    zombieState = ZombieState.walking;
                }
                break;

            default:
                break;
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
        zomhealth.gotShoot = false;
        animator.SetBool("Flinch", false);
    }

  /*  IEnumerator Zombie()
    {
        while (true)
        {
           
            switch (zombieState)
            {
                case ZombieState.idle:
                    animator.SetBool("Idle", true);
                    if (distance <= detectionDistance)
                    {
                        animator.SetBool("Idle", false);
                        zombieState = ZombieState.walking;
                    }
                    nav.SetDestination(transform.position);
                    break;

                case ZombieState.walking:
                    animator.SetBool("Walking", true);
                    nav.SetDestination(target.position);
                    if (distance <= stopDistance)
                    {
                        animator.SetBool("Walking", false);
                        zombieState = ZombieState.attacking;
                    }
                   
                    break;

                case ZombieState.running:
                    animator.SetBool("Running", true);
                    break;

                case ZombieState.crawling:
                    animator.SetBool("Crawling", true);
                    break;

                case ZombieState.attacking:
                    animator.SetBool("Attacking", true);
                    if (distance > stopDistance)
                    {
                        animator.SetBool("Attacking", false);
                        zombieState = ZombieState.walking;
                    }

                    break;

                default:
                    break;
            }
            yield return new WaitForSeconds(1);
        }
    }
    */
}
