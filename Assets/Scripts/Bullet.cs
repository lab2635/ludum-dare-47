using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Gun source;
    public GameObject DetonatorPrefab;

    public float speed = 11f;

    public Boolean isPlayerBullet;
    public AudioClip ShootSFX;
    public AudioClip LaserDeathSFX;

    private AudioSource audioSource;

    private void Start()
    {
        if (this.audioSource == null)
        {
            this.audioSource = gameObject.AddComponent<AudioSource>();
        }
        this.audioSource.clip = this.ShootSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
    }

    private void OnEnable()
    {
        if (this.audioSource == null)
        {
            this.audioSource = gameObject.AddComponent<AudioSource>();
        }
        this.audioSource.clip = this.ShootSFX;

        if (!this.isPlayerBullet)
        {
            this.audioSource.spread = 180;
            this.audioSource.maxDistance = 5;
            this.audioSource.volume = 0.05f;
        }

        this.audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        source.Hit(this, other);
    }
    
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Explode()
    {
        var bulletContainer = GameObject.FindGameObjectWithTag("Bullets");
        
        Detonator dTemp = (Detonator)this.DetonatorPrefab.GetComponent("Detonator");
        GameObject exp = (GameObject)Instantiate(this.DetonatorPrefab, this.
            transform.position, Quaternion.identity, bulletContainer.transform);
        
        dTemp = (Detonator)exp.GetComponent("Detonator");
        dTemp.detail = 1.0f;

        Destroy(exp, 10);
    }

    public void PlayerImpact()
    {
        this.audioSource.clip = this.LaserDeathSFX;
        this.audioSource.volume = 10f;
        this.audioSource.Play();
    }
}
