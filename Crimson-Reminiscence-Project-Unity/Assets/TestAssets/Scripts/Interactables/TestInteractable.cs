using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;
    public bool canBePressed = true;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] interact = default;

    Animator anim;
    public static TestInteractable Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if(canBePressed == true)
        {
            audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
            canBePressed = false;
            hasBeenPressed = true;
            anim.SetBool("Pressed", true);
            print("Interacted with " + gameObject.name);
        }
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
