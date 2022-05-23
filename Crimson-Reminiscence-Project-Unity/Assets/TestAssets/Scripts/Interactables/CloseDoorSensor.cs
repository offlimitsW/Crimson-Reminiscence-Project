using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorSensor : MonoBehaviour
{
    public bool playerEntered = false;
    public bool playerHasEntered = false;
    public static CloseDoorSensor Instance;

    void Start()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }
}
