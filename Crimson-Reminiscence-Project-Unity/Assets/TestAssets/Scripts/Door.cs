using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorAudio;
    public GameObject closeDoorAudio;
    public GameObject zombie;

    [Header("Spawn Zombie Locations")]
    public Transform zombieSpawnOrigin;
    public Transform zombieSpawnOrigin1;
    public Transform zombieSpawnOrigin2;

    Animator anim;
    public static Door Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(TestInteractable.Instance.hasBeenPressed == true)
        {
            TestInteractable.Instance.hasBeenPressed = false;
            anim.SetBool("IsOpening", true);
            Instantiate(zombie, zombieSpawnOrigin.transform.position, transform.rotation);
            StartCoroutine(Open());
        }

        if (CloseDoorSensor.Instance.playerEntered == true && CloseDoorSensor.Instance.playerHasEntered == false)
        {
            CloseDoorSensor.Instance.playerHasEntered = true;
            anim.SetBool("Close", true);
            Instantiate(zombie, zombieSpawnOrigin2.transform.position, transform.rotation);
            closeDoorAudio.SetActive(true);
        }
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(0.25f);
        doorAudio.SetActive(true);
    }
}
