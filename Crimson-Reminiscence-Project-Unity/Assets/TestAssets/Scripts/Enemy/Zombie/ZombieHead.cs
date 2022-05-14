using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    public float health = 1f;
    public GameObject head;
    public GameObject splat;
    public GameObject splatAudio;

    public static ZombieHead Instance;

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {

    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            splat.SetActive(true);
            splatAudio.SetActive(true);
            ZombieAI.Instance.hasHead = false;
            ZombieAI.Instance.health = 0f;
            Die();
        }
    }

    void Die()
    {
        head.SetActive(false);
    }
}
