using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RayCastPistol : MonoBehaviour
{
    public bool disable = false;

    [Header("Bullet Spawn/Destination")]
    public Transform rayCastOrigin;
    public Transform rayCastDestination;
    public Transform casingOrigin;

    [Header("Pistol VFX/SFX")]
    public GameObject lightFlash;
    public GameObject muzzleFlash;
    public GameObject muzzleFlash1;
    public GameObject hitEffect;
    public GameObject hitEffect1;
    public GameObject hitZombieEffect;
    public GameObject hitSmoke;
    public TrailRenderer tracerEffect;
    public GameObject casing;
    [SerializeField] private AudioSource shootAudioSource = default;
    [SerializeField] private AudioClip[] shoot = default;
    [SerializeField] private AudioClip[] empty = default;
    [SerializeField] private AudioClip[] aim = default;
    [SerializeField] private AudioClip[] reload = default;

    [Header("Ammo")]
    public float bulletCount = 12f;
    public bool canShoot = true;
    public bool canReload = false;

    [Header("Paramaters")]
    public float fireRate = 25f;
    private float nextTimeToFire = 0f;
    public float damage = 10f;
    public float impactForce = -80f;

    [Header("Animations")]
    public bool isRun;
    public bool isAiming;
    public bool isReloading = false;

    [Header("Controls")]
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    Ray ray;
    RaycastHit hitInfo;
    Animator anim;
    public static RayCastPistol Instance;

    [SerializeField] private TextMeshProUGUI currentAmmoText = default;
    [SerializeField] private TextMeshProUGUI storedAmmoText = default;

    private void Start()
    {
        UpdateAmmo(12);
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (disable)
            return;
        currentAmmoText.text = bulletCount.ToString("0");

        if (Input.GetKeyDown(zoomKey))
        {
            shootAudioSource.PlayOneShot(aim[UnityEngine.Random.Range(0, aim.Length - 1)]);
            isAiming = true;
            anim.SetBool("ADS", true);
        }
        if (Input.GetKeyUp(zoomKey))
        {
            isAiming = false;
            anim.SetBool("ADS", false);
            anim.SetBool("ADSFire", false);
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && canShoot == true)
        {
            bulletCount--;
            if (isAiming == true)
                anim.SetBool("ADSFire", true);
            else
            {
                anim.SetBool("ADSFire", false);
                anim.SetTrigger("Shoot");
            }

            shootAudioSource.PlayOneShot(shoot[UnityEngine.Random.Range(0, shoot.Length - 1)]);
            Instantiate(muzzleFlash, rayCastOrigin.transform.position, transform.rotation);
            Instantiate(muzzleFlash1, rayCastOrigin.transform.position, transform.rotation);
            Instantiate(lightFlash, rayCastOrigin.transform.position, transform.rotation);
            Instantiate(casing, casingOrigin.transform.position, transform.rotation);
            nextTimeToFire = Time.time + 8f / fireRate;
            ray.origin = rayCastOrigin.position;
            ray.direction = rayCastDestination.position - rayCastOrigin.position;

            var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

                ZombieAI target = hitInfo.transform.GetComponent<ZombieAI>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                ZombieHead target1 = hitInfo.transform.GetComponent<ZombieHead>();
                if (target1 != null)
                {
                    target1.TakeDamage(damage);
                }

                if (hitInfo.rigidbody != null)
                {
                    GameObject impactGO1 = Instantiate(hitZombieEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactGO1, 4f);
                    hitInfo.rigidbody.AddForce(hitInfo.normal * impactForce);
                }

                if (!hitInfo.rigidbody)
                {
                    GameObject impactGO = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactGO, 12f);
                    GameObject impactGO1 = Instantiate(hitEffect1, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactGO1, 4f);
                    GameObject impactGO2 = Instantiate(hitSmoke, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactGO2, 2f);
                }

                tracer.transform.position = hitInfo.point;
            }

        }
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && canShoot == false && isReloading == false)
        {
            anim.SetTrigger("Empty");
            shootAudioSource.PlayOneShot(empty[UnityEngine.Random.Range(0, empty.Length - 1)]);
        }

        if (bulletCount == 0)
            canShoot = false;

        if (bulletCount <= 11 && PlayerController.Instance.isSprinting == false)
        {
            canReload = true;
        }
        else if (bulletCount > 11)
        {
            canReload = false;
        }

        if (Input.GetKeyDown(reloadKey) && canReload == true && isReloading == false)
        {
            isReloading = true;
            canShoot = false;
            StartCoroutine(ReloadFunction());
        }


        if (PlayerController.Instance.isSprinting == true)
        {
            isRun = true;
            anim.SetBool("Running", true);
        }
        else if (PlayerController.Instance.isSprinting == false)
        {
            isRun = false;
            anim.SetBool("Running", false);
        }
    }

    private void UpdateAmmo(float currentAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString("0");
    }

    IEnumerator ReloadFunction()
    {
        shootAudioSource.PlayOneShot(reload[UnityEngine.Random.Range(0, reload.Length - 1)]);
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(2.5f);
        bulletCount = 12f;
        canShoot = true;
        isReloading = false;
    }
}
