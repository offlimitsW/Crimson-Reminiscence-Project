using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
    public bool labDoorOpen = false;
    Animator anim;
    public static LabDoor Instance;
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();

    }
    void Update()
    {
        if(GameManager.Instance.labDoorOpen == true)
            anim.SetBool("Open", true);
    }
}
