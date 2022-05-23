using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLever1 : MonoBehaviour
{

    Animator anim;
    public static PLever1 Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PowerLever1.Instance.hasBeenPressed == true)
        {
            anim.SetBool("Pressed", true);
        }
    }
}
