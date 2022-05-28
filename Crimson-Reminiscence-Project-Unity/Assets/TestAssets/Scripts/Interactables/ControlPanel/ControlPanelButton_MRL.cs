using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelButton_MRL : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;
    public bool canBeenPressed = true;
    public bool button_MRLPressed = false;
    public GameObject activateButton;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioSource screenAudioSource = default;
    [SerializeField] private AudioClip[] interact = default;
    [SerializeField] private AudioClip[] screenPassed = default;
    [SerializeField] private AudioClip[] denied = default;


    [Header("All Green Buttons")]
    public GameObject On_LL;
    public GameObject On_ML;
    public GameObject On_TL;
    public GameObject On_MT;
    public GameObject On_TR;
    public GameObject On_MR;
    public GameObject On_LR;
    public GameObject On_MB;

    Animator anim;
    public static ControlPanelButton_MRL Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (hasBeenPressed == false)
            anim.SetBool("Pressed", false);
    }

    public override void OnInteract()
    {
        if (ControlPanelButton_ML.Instance.hasBeenPressed == true && ControlPanelMainFrame.Instance.mainFramePowered == true && hasBeenPressed == false && canBeenPressed == true)
        {
            audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
            hasBeenPressed = true;
            button_MRLPressed = true;
            anim.SetBool("Pressed", true);
            StartCoroutine(ButtonActivated());
            print("Interacted with " + gameObject.name);
        }
        else if(ControlPanelMainFrame.Instance.mainFramePowered == true && hasBeenPressed == false)
        {
            ControlPanelButton_LB.Instance.hasBeenPressed = false;
            ControlPanelButton_LB.Instance.canBeenPressed = true;
            ControlPanelButton_LB.Instance.button_LBPressed = false;

            ControlPanelButton_ML.Instance.hasBeenPressed = false;
            ControlPanelButton_ML.Instance.canBeenPressed = true;
            ControlPanelButton_ML.Instance.button_MLPressed = false;

            audioSource.PlayOneShot(denied[UnityEngine.Random.Range(0, interact.Length - 1)]);
            On_LL.SetActive(false);
            On_ML.SetActive(false);
            On_TL.SetActive(false);
            On_MT.SetActive(false);
            On_TR.SetActive(false);
            On_MR.SetActive(false);
            On_LR.SetActive(false);
            On_MB.SetActive(false);
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
