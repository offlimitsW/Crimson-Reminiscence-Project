using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Lab Entrance Lights")]
    public bool powerOn = false;
    public GameObject lightSource;
    public GameObject lightSource1;
    public GameObject lightSource2;
    public GameObject lightSource3;
    public GameObject lightSource4;
    public GameObject lightSource5;

    [Header("Lab Entrance Control Panel")]
    public GameObject ComputerAudio;
    public GameObject controlPanel;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(PowerLever.Instance.leverPower == true && PowerLever1.Instance.leverPower1 == true && PowerLever2.Instance.leverPower2 == true)
        {
            powerOn = true;

            StartCoroutine(LightsOn());
        }
    }

    private IEnumerator LightsOn()
    {
        yield return new WaitForSeconds(2.0f);
        lightSource.SetActive(true);
        lightSource1.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource2.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource3.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource4.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource5.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        ComputerAudio.SetActive(true);
        ControlPanelMainFrame.Instance.mainFramePowered = true;
        controlPanel.SetActive(true);
    }
}
