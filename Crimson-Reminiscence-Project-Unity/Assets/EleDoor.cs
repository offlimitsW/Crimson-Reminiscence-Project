using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleDoor : MonoBehaviour
{
    public GameObject doorAudio;

    Animator anim;
    public static EleDoor Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (ElevatorButton.Instance.hasBeenPressed == true)
        {
            TestInteractable.Instance.hasBeenPressed = false;
            anim.SetBool("Open", true);
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(0.25f);
        doorAudio.SetActive(true);
    }
}
