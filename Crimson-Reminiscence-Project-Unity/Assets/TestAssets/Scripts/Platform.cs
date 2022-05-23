using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool powered = false;
    public GameObject platformAudio;

    Animator anim;
    public static Platform Instance;

    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(powered == true)
        {
            anim.SetBool("isPowered", true);
            platformAudio.SetActive(true);
        }
    }
}
