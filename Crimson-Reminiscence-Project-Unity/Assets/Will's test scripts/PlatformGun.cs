using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGun : MonoBehaviour
{
    public bool canShoot = true;
    public float range = 100f;

    public GameObject platformPrefab;
    public Material solidPlatformMaterial;

    void Update()
    {
        // When fire, check if the collided object are able to Instantiate platformPrefab
        if (Input.GetButton("Fire1") && canShoot == true)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                var selection = hit.transform;
                if (selection.GetComponent<PlatformAddonAble>() != null)
                {
                    print("Raycast hit " + selection.name);
                    Instantiate(platformPrefab, selection.position, selection.rotation);
                }

                if (selection.GetComponent<GhostPlatform>() != null)
                {
                    print("Raycast hit " + selection.name);

                    selection.GetComponent<GhostPlatform>().Activate();

                    // Reminder: add a method in GhostPlatform to switch MAT
                    //if (selection.GetComponentInChildren<Renderer>() != null)
                    //{
                    //    selection.GetComponentInChildren<Renderer>().material = solidPlatformMaterial;
                    //}
                    //if (selection.GetComponent<BoxCollider>() != null)
                    //{
                    //    selection.GetComponent<BoxCollider>().isTrigger = false;
                    //}
                }

                else print("cant add platform here");
            }
        }
    }
}
