using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;

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
        audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
        hasBeenPressed = true;
        anim.SetBool("Pressed", true);
        print("Interacted with " + gameObject.name);
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
