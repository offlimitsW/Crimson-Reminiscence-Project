using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelButton_LB : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;
    public bool canBeenPressed = true;
    public bool button_LBPressed = false;
    public GameObject activateButton;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioSource screenAudioSource = default;
    [SerializeField] private AudioClip[] interact = default;
    [SerializeField] private AudioClip[] screenPassed = default;
    [SerializeField] private AudioClip[] denied = default;

    Animator anim;
    public static ControlPanelButton_LB Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if (ControlPanelMainFrame.Instance.mainFramePowered == true && hasBeenPressed == false && canBeenPressed == true)
        {
            audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
            hasBeenPressed = true;
            button_LBPressed = true;
            anim.SetBool("Pressed", true);
            StartCoroutine(ButtonActivated());
            print("Interacted with " + gameObject.name);
        }
    }

    private IEnumerator ButtonActivated()
    {
        yield return new WaitForSeconds(0.25f);
        screenAudioSource.PlayOneShot(screenPassed[UnityEngine.Random.Range(0, interact.Length - 1)]);
        activateButton.SetActive(true);
    }

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
    }
    public override void OnLoseFocus()
    {
        print("Stopped Looking at " + gameObject.name);
    }
}
