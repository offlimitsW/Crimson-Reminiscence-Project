using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1.0f;

    public GameObject impactPrefab = null;
    public GameObject impactPrefab1 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*speed);
        Destroy(gameObject, 4);
    }

    void OnTriggerEnter(Collider other)
    {
        Instantiate(impactPrefab, transform.position, transform.rotation);
        Instantiate(impactPrefab1, transform.position, transform.rotation);
        Destroy(gameObject, 0.1f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(impactPrefab, transform.position, transform.rotation);
        Instantiate(impactPrefab1, transform.position, transform.rotation);
        Destroy(gameObject, 0.1f);
    }
}
