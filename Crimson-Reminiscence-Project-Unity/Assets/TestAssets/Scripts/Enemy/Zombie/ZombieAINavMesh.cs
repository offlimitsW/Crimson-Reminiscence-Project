using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAINavMesh : MonoBehaviour
{
    public GameObject head;
    public bool hasHead = true;
    public bool isAlive = true;
    public bool isCorpse = false;

    public float deadPos;
    public float speed = 3f;
    public float health = 50f;
    public float hitForce = -30f;

    Animator anim;
    public static ZombieAINavMesh Instance;
    RaycastHit hitPlayer;
    private Vector3 hitPointNormal;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBeforeAttacks;
    public bool isAttacking = false;
    public bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        deadPos = transform.localPosition.y;
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(isAlive == true)
        {
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInSightRange) AttackPlayer();
        }

        if (isAlive == true)
        {

        }

        if (isCorpse == false || isAttacking == false)
        {
        }

        if (health <= 0f)
        {
            Die();
        }

        Debug.DrawRay(transform.position, Vector3.back, Color.red);
        if (isAlive == true && Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitPlayer, 2f))
        {
            hitPointNormal += hitPlayer.normal;

            PlayerController target = hitPlayer.transform.GetComponent<PlayerController>();
            if (target != null)
            {
            }

            if (hitPlayer.rigidbody != null)
            {
                isAttacking = true;
                StartCoroutine(Attack());
                hitPlayer.rigidbody.AddForce(hitPlayer.normal * hitForce);
            }
        }
    }

    IEnumerator Attack()
    {
        speed = 0f;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        if (isAlive == true && Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitPlayer, 2f))
        {
            hitPointNormal += hitPlayer.normal;

            PlayerController target = hitPlayer.transform.GetComponent<PlayerController>();
            if (target != null)
            {
            }

            if (hitPlayer.rigidbody != null)
            {
                isAttacking = true;
                PlayerController.onTakeDamage(1);
            }
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
        speed = 3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
            TakeDamage(3);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            isAlive = false;
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        speed = 0f;

        if(hasHead == false)
            anim.SetTrigger("NoHead");
        else
            anim.SetTrigger("Dead");

        StartCoroutine(Corpse());
    }

    IEnumerator Corpse()
    {
        yield return new WaitForSeconds(3f);
        isCorpse = true;
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBeforeAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
