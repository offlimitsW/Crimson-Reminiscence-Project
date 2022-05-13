using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour

{
    public GameObject projectile;
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;


    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && GunPistol.Instance.canShoot == true)
        {
            nextTimeToFire = Time.time + 8f / fireRate;
            Instantiate(projectile, transform.position, transform.rotation);
        }
    }
}
