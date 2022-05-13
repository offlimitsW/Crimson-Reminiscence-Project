using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [Header("Casing SFX")]
    [SerializeField] private AudioSource impactAudioSource = default;
    [SerializeField] private AudioClip[] impact = default;

    public void OnCollisionEnter(Collision collision)
    {
        impactAudioSource.PlayOneShot(impact[UnityEngine.Random.Range(1, impact.Length - 1)]);
        Destroy(gameObject, 8f);
    }

    public void OnTriggerEnter(Collider other)
    {
        impactAudioSource.PlayOneShot(impact[UnityEngine.Random.Range(1, impact.Length - 1)]);
        Destroy(gameObject, 2f);
    }
}
