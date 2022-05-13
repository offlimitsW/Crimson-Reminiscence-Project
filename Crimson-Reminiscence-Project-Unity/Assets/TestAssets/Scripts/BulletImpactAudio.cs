using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactAudio : MonoBehaviour
{
    [SerializeField] private AudioSource impactAudioSource = default;
    [SerializeField] private AudioClip[] impact = default;

    void Awake()
    {
        impactAudioSource.PlayOneShot(impact[UnityEngine.Random.Range(0, impact.Length - 1)]);
    }
}
