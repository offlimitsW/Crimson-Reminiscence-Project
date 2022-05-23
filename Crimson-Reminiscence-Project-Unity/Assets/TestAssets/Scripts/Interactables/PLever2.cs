using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLever2 : MonoBehaviour
{

    Animator anim;
    public static PLever2 Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PowerLever2.Instance.hasBeenPressed == true)
        {
            anim.SetBool("Pressed", true);
        }
    }
}
