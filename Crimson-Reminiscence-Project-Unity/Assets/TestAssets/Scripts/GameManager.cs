using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Lab Entrance Lights")]
    public bool powerOn = false;
    public GameObject redLight;
    public GameObject lightSource;
    public GameObject lightSource1;
    public GameObject lightSource2;
    public GameObject lightSource3;
    public GameObject lightSource4;
    public GameObject lightSource5;
    public GameObject lightSource6;

    [Header("Lab Entrance Control Panel")]
    [SerializeField] private AudioSource screenAudioSource = default;
    [SerializeField] private AudioClip[] passed = default;
    public GameObject ComputerAudio;
    public GameObject controlPanel;
    public GameObject controlPanelPassed;
    public bool playPassed = true;
    public bool labDoorOpen = false;

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

        if (playPassed == true && ControlPanelButton_TL.Instance.hasBeenPressed == true && ControlPanelButton_RL.Instance.hasBeenPressed == true && ControlPanelButton_MLL.Instance.hasBeenPressed == true && ControlPanelButton_MR.Instance.hasBeenPressed == true && ControlPanelButton_RT.Instance.hasBeenPressed == true && ControlPanelButton_MRL.Instance.hasBeenPressed == true && ControlPanelButton_ML.Instance.hasBeenPressed == true && ControlPanelButton_LB.Instance.hasBeenPressed == true)
        {
            StartCoroutine(WalkWay());
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
        redLight.SetActive(false);
        lightSource6.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource4.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        lightSource5.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        ComputerAudio.SetActive(true);
        ControlPanelMainFrame.Instance.mainFramePowered = true;
        controlPanel.SetActive(true);
    }

    private IEnumerator WalkWay()
    {
        playPassed = false;
        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(false);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(true);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(false);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(true);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(false);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(true);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(false);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);

        yield return new WaitForSeconds(0.5f);
        controlPanelPassed.SetActive(true);
        screenAudioSource.PlayOneShot(passed[UnityEngine.Random.Range(0, passed.Length - 1)]);
        Platform.Instance.powered = true;

        yield return new WaitForSeconds(2f);
        labDoorOpen = true;
    }
}
