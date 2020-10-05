using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public AudioClip AcidDeathSFX;

    private AudioSource audioSource;

    private void Start()
    {
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.AcidDeathSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.audioSource.Play();
            GameManager.Instance.KillRespawnPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.KillRespawnPlayer();
        }
    }
}
