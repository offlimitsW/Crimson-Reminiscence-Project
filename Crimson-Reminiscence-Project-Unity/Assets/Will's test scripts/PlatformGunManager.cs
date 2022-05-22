using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGunManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The amount of platforms created from platform gun that can exist simutaneously")]
    int amountAllowed;

    GameObject[] platformArray;

    void Start()
    {
        platformArray = new GameObject[amountAllowed];
    }

    public void AddNewPlatform(GameObject newPlatform)
    {
        // if there is a null index, add the new one at that index
        for (int i = 0; i < platformArray.Length; i++)
        {
            if (platformArray[i] == null)
            {
                platformArray[i] = newPlatform;
                return;
            }
        }
        // else (full array), delete GameObject at index 0, move all the other platforms up by 1, then add the new one at the last index
        Destroy(platformArray[0]);
        for (int i = 0; i < platformArray.Length - 1; i++) 
        {
            platformArray[i] = platformArray[i + 1];
        }
        platformArray[platformArray.Length - 1] = newPlatform;
    }
}
