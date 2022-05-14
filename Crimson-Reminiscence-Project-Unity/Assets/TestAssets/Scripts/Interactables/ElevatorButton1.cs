using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton1 : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;
    public bool goUp = false;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] interact = default;
    [SerializeField] private AudioClip[] denied = default;

    Animator anim;
    public static ElevatorButton1 Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if(hasBeenPressed == false)
        {
            audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
            hasBeenPressed = true;
            anim.SetBool("Pressed", false);
            print("Interacted with " + gameObject.name);
            StartCoroutine(EleUp());
        }
    }

    IEnumerator EleUp()
    {
        yield return new WaitForSeconds(1.5f);
        goUp = true;
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
