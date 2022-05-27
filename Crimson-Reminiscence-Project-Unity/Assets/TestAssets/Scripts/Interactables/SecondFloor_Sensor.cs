using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloor_Sensor : MonoBehaviour
{
    public bool playerEntered = false;
    public bool playerHasEntered = false;
    public static SecondFloor_Sensor Instance;

    [Header("Upper Lights Off")]
    public GameObject flickerLight;
    public GameObject doorLight;

    [Header("Zombie")]
    public GameObject zombie;

    [Header("Zombie Spawn Locations")]
    public Transform zombieSpawnOrigin;

    void Start()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerEntered == false)
        {
            playerEntered = true;
            flickerLight.SetActive(false);
            doorLight.SetActive(false);
            Instantiate(zombie, zombieSpawnOrigin.transform.position, transform.rotation);
        }
    }
}
