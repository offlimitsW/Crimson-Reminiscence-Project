using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastPistolAC : MonoBehaviour
{
    [Header("Animations")]
    public bool isRunning;
    public bool isAiming;
    public bool isReloading = false;

    Animator anim;

    [Header("Controls")]
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            isAiming = true;
            anim.SetBool("ADS", true);
        }
        else if (Input.GetKeyUp(zoomKey))
        {
            isAiming = false;
            anim.SetBool("ADS", false);
        }
    }
}
