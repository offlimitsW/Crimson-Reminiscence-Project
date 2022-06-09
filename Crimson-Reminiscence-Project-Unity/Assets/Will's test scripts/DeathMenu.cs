using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public static bool isOnDeathScreen = false;
    public GameObject deathMenuUI;
    public PauseMenu pauseMenuScript;

    public PlayerController playerController;
    public RayCastPistol pistolScript;
    public PlatformGun platformGunScript;

    public void ActivateDeathScreen()
    {
        if(!isOnDeathScreen)
        {
            playerController.disable = true;
            pistolScript.disable = true;
            platformGunScript.disable = true;
            isOnDeathScreen = true;
            deathMenuUI.SetActive(true);
            pauseMenuScript.disable = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void DeactivateDeathScreen()
    {
        if (isOnDeathScreen)
        {
            playerController.disable = false;
            pistolScript.disable = false;
            platformGunScript.disable = false;
            isOnDeathScreen = false;
            deathMenuUI.SetActive(false);
            pauseMenuScript.disable = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
