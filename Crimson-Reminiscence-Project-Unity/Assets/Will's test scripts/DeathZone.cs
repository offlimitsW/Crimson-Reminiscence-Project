using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    DeathMenu deathMenu;

    private void Awake()
    {
        deathMenu = GameObject.Find("UI").GetComponent<DeathMenu>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathMenu.ActivateDeathScreen();
        }
    }
}
