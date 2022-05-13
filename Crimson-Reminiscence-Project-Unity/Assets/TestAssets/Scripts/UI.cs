using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText = default;

    private void OnEnable()
    {
        PlayerController.onDamage += UpdateHealth;
        PlayerController.onHeal += UpdateHealth;
    }

    private void OnDisable()
    {
        PlayerController.onDamage -= UpdateHealth;
        PlayerController.onHeal -= UpdateHealth;
    }

    private void UpdateHealth(float currentHealth)
    {
        healthText.text = currentHealth.ToString("00");
    }

    private void Start()
    {
        UpdateHealth(100);
    }
}
