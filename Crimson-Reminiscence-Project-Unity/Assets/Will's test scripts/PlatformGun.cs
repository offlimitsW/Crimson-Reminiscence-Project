using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGun : MonoBehaviour
{
    public bool disable;

    public bool canShoot = true;
    public float range = 100f;

    [SerializeField] GameObject platformPrefab;
    // Maybe the player can have different sizes of platform to choose from?

    PlatformGunManager platformGunManager;

    private void Start()
    {
        platformGunManager = GetComponent<PlatformGunManager>();
    }

    void Update()
    {
        if (disable)
            return;
        // When fire, check if the collided object are able to Instantiate platformPrefab
        if (Input.GetButtonDown("Fire1") && canShoot == true)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                var selection = hit.transform;
                if (selection.GetComponent<PlatformAddonAble>() != null)
                {
                    print("Raycast hit " + selection.name + " at position: " + hit.point);
                    platformGunManager.AddNewPlatform(Instantiate(platformPrefab, hit.point, hit.transform.rotation));
                }
                if (selection.GetComponent<GhostPlatform>() != null)
                {
                    print("Raycast hit " + selection.name);
                    selection.GetComponent<GhostPlatform>().Activate();
                }
                else print("Can't add platform here");
            }
        }
    }
}
