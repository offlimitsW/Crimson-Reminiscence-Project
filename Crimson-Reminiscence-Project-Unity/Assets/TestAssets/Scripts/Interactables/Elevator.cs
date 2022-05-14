using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Animator anim;
    public static Elevator Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(ElevatorButton1.Instance.goUp == true)
            anim.SetBool("Up", true);
    }
}
