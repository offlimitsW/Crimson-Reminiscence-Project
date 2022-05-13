using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("Paramaters")]
    private float fireRate = 60f;
    private float nextTimeToFire = 0f;

    private float currentAmmo = 12f;
    private float storedAmmo = 12f;

    [SerializeField] private TextMeshProUGUI currentAmmoText = default;
    [SerializeField] private TextMeshProUGUI ammoStoredText = default;

    public void update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            UpdateAmmo(1);
        }
    }

    public void UpdateAmmo(float currentAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString("0");
    }

    private void Start()
    {
        UpdateAmmo(12);
    }
}
