using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool disable;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public PlayerController playerController;
    public RayCastPistol pistolScript;
    public PlatformGun platformGunScript;
    SpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        if (disable)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        playerController.disable = false;
        pistolScript.disable = false;
        platformGunScript.disable = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        playerController.disable = true;
        pistolScript.disable = true;
        platformGunScript.disable = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void QuitGame()
    {
        print("quitting game");
        Application.Quit();
    }

    public void RespawnLastCheckpoint()
    {
        StartCoroutine(spawnManager.SpawnPlayer());
        Resume();
    }
}
