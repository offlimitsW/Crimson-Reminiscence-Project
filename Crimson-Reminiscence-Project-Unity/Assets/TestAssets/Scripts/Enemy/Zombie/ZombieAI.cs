using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public GameObject head;
    public bool hasHead = true;
    public bool isAlive = true;
    public bool isCorpse = false;
    public bool isAttacking = false;

    public float speed = 3f;
    public float health = 50f;
    public float hitForce = -30f;

    Animator anim;
    public static ZombieAI Instance;
    RaycastHit hitPlayer;
    private Vector3 hitPointNormal;
    private float rotationX = 0;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if (isAlive == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if (isCorpse == false)
        {
            transform.LookAt(player.gameObject.transform.position);
        }

        if (health <= 0f)
        {
            Die();
        }

        Debug.DrawRay(transform.position, Vector3.back, Color.red);
        if (isAlive == true && Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitPlayer, 1f))
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
        if (isAlive == true && Physics.Raycast(transform.position, Vector3.back, out RaycastHit hitPlayer, 1f))
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
        yield return new WaitForSeconds(1f);
        isCorpse = true;
        rotationX = transform.localRotation.x;
    }
}
