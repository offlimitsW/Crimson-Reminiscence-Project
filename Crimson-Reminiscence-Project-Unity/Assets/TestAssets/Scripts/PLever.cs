using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLever : MonoBehaviour
{

    Animator anim;
    public static PLever Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PowerLever.Instance.hasBeenPressed == true)
        {
            anim.SetBool("Pressed", true);
        }
    }
}
