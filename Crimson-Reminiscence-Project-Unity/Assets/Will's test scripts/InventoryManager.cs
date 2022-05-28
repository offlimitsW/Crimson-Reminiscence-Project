using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventory;

    // Just so it runs
    public GameObject startingGun;
    public GameObject shootScript;

    void Start()
    {
        // Ideally, I would set up a Save and Load method into a database but lol
        inventory.Add(startingGun);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(0);          
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(1);
        }
    }

    public void SelectWeapon(int index)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (i != index)
            { 
                inventory[i].SetActive(false);
            }
            else inventory[i].SetActive(true);
        }
        // lol lmao
        if (index == 0)
            shootScript.GetComponent<RayCastPistol>().enabled = true;
        else
            shootScript.GetComponent<RayCastPistol>().enabled = false;
    }

    public void AddWeapon(GameObject newWeapon)
    {
        inventory.Add(newWeapon);
        SelectWeapon(inventory.Count - 1);
    }
}
