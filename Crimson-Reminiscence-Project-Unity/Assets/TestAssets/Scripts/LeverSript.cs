using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverSript : MonoBehaviour
{

    Animator anim;
    public static LeverSript Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (EBoxLever.Instance.hasBeenPressed == true)
        {
            ElevatorButton.Instance.isPowered = true;
            anim.SetBool("Pressed", true);
        }
    }
}
