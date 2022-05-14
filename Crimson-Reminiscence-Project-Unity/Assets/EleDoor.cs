using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleDoor : MonoBehaviour
{
    public GameObject doorAudio;
    public GameObject doorAudio2;
    public GameObject closeDoorAudio;

    Animator anim;
    public static EleDoor Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (ElevatorButton.Instance.hasBeenPressed == true && ElevatorButton.Instance.canBeenPressed == true)
        {
            TestInteractable.Instance.hasBeenPressed = false;
            ElevatorButton.Instance.canBeenPressed = false;
            anim.SetBool("Open", true);
            StartCoroutine(Open());
        }
        if (ElevatorButton1.Instance.hasBeenPressed == true && ElevatorButton1.Instance.canGoUp == true)
        {
            TestInteractable.Instance.hasBeenPressed = false;
            ElevatorButton1.Instance.canGoUp = false;
            anim.SetBool("Open", false);
            closeDoorAudio.SetActive(true);
            StartCoroutine(Open2());
        }
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(0.25f);
        doorAudio.SetActive(true);
    }
    IEnumerator Open2()
    {
        yield return new WaitForSeconds(10f);
        anim.SetBool("Open", true);
        doorAudio2.SetActive(true);
    }
}
