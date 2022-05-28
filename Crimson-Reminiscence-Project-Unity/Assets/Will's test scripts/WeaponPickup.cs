using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public InventoryManager inventoryManager;

    [Tooltip("The game object i nested in the player character")]
    public GameObject actualWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if (inventoryManager != null)
        {
            inventoryManager.AddWeapon(actualWeapon);
            Destroy(gameObject);
        }
    }
}
