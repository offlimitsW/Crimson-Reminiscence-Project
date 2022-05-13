using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPistol : MonoBehaviour
{
    [Header("Pistol Options")]
    [SerializeField] public bool canShoot = true;
    [SerializeField] private float bulletCount = 4;
    [SerializeField] private float timeBeforeAmmoRegen = 0.25f;
    [SerializeField] private bool bulletRegenStart = true;
    public GameObject projectile;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 80f;
    public Camera fpsCam;
    public float fieldOfView = 60f;

    [Header("Controls")]

    [Header("Pistol Ammo Display")]
    public GameObject bullet;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;

    Animator anim;
    public static GunPistol Instance;

    [Header("Pistol VFX/SFX")]
    public GameObject muzzleFlash;
    public GameObject rechargeSmoke;
    public GameObject rechargeSmoke1;
    public GameObject shootAudio;
    public GameObject emptyAudio;
    public GameObject reloadAudio;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && canShoot == true)
        {
            bulletCount--;
            nextTimeToFire = Time.time + 8f / fireRate;
            StartCoroutine(Shoot());
            muzzleFlash.SetActive(true);
            anim.SetTrigger("Shoot");
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && bulletCount == 0)
        {
            Instantiate(emptyAudio);
            nextTimeToFire = Time.time + 8f / fireRate;
        }

        if (bulletCount == 3)
        {
            bullet.SetActive(false);
        }
        if (bulletCount == 2)
        {
            bullet1.SetActive(false);
        }
        if (bulletCount == 1)
        {
            bullet2.SetActive(false);
        }
        if (bulletCount == 0 && bulletRegenStart == true)
        {
            canShoot = false;
            bullet3.SetActive(false);
            bulletRegenStart = false;

            if(bulletRegenStart == false)
                StartCoroutine(RegenAmmo());
        }
    }

    IEnumerator Shoot()
    {
        Instantiate(shootAudio);
        yield return new WaitForSeconds(0.25f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator RegenAmmo()
    {
        yield return new WaitForSeconds(0.5f);
        rechargeSmoke.SetActive(true);
        rechargeSmoke1.SetActive(true);
        Instantiate(reloadAudio);
        bullet3.SetActive(true);

        yield return new WaitForSeconds(timeBeforeAmmoRegen);
        bullet2.SetActive(true);

        yield return new WaitForSeconds(timeBeforeAmmoRegen);
        bullet1.SetActive(true);

        yield return new WaitForSeconds(timeBeforeAmmoRegen);
        bullet.SetActive(true);

        yield return new WaitForSeconds(timeBeforeAmmoRegen);
        bulletCount = 4;
        bulletRegenStart = true;

        yield return new WaitForSeconds(0.25f);
        canShoot = true;

        yield return new WaitForSeconds(0.5f);
        rechargeSmoke.SetActive(false);
        rechargeSmoke1.SetActive(false);
    }
}
