using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBoxLever : Interactable
{
    [Header("Paramaters")]
    public bool hasBeenPressed = false;

    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip[] interact = default;
    public GameObject generatorAudio;

    Animator anim;
    public static EBoxLever Instance;

    private void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if(hasBeenPressed == false)
        {
            StartCoroutine(GeneratorAudio());
        }

        hasBeenPressed = true;
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

    private IEnumerator GeneratorAudio()
    {
        yield return new WaitForSeconds(0.75f);
        audioSource.PlayOneShot(interact[UnityEngine.Random.Range(0, interact.Length - 1)]);
        yield return new WaitForSeconds(1.25f);
        generatorAudio.SetActive(true);
    }
}
