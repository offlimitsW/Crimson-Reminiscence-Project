using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelMainFrame : MonoBehaviour
{
    [Header("Paramaters")]
    public bool mainFramePowered = false;

    public static ControlPanelMainFrame Instance;

    private void Start()
    {
        Instance = this;
    }
}
