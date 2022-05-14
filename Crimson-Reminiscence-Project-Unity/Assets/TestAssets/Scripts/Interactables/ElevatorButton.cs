using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;
    public bool canBeenPressed = true;
    public bool isPowered = false;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] interact = default;
    [SerializeField] private AudioClip[] denied = default;

    Animator anim;
    public static ElevatorButton Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if(isPowered == true && hasBeenPressed == false && canBeenPressed == true)
        {
            audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
            hasBeenPressed = true;
            anim.SetBool("Pressed", true);
            print("Interacted with " + gameObject.name);
        }
        else if (isPowered == false)
            audioSource.PlayOneShot(denied[UnityEngine.Random.Range(0, denied.Length - 1)]);
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
