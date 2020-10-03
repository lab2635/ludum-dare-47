using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Nody.Nodes;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Bullet[] bullets;
    private float lastFireAccumulator = 0f;
    private int bulletIndex;
    private float computedFireRate;

    private const string BulletTag = "Bullet";
    
    [SerializeField]
    public float fireRate = 5f;
    [SerializeField]
    public Bullet bulletPrefab;

    public void Hit(Bullet bullet, Collider collision)
    {
        if (!collision.CompareTag(BulletTag))
        {
            Return(bullet);
        }
    }

    void Return(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    void Start()
    {
        bullets = new Bullet[100];
        computedFireRate = 1 / fireRate;
        
        for (var i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].source = this;
            bullets[i].transform.position = transform.position;
            bullets[i].transform.rotation = transform.rotation;
            bullets[i].gameObject.SetActive(false);
        }
    }

    void Fire(int index = 0)
    {
        bullets[index].transform.position = transform.position;
        bullets[index].transform.rotation = transform.rotation;
        bullets[index].gameObject.SetActive(true);
        bulletIndex = (bulletIndex + 1) % bullets.Length;
    }

    void Update()
    {
        lastFireAccumulator += Time.deltaTime;

        if (lastFireAccumulator >= computedFireRate)
        {
            lastFireAccumulator = 0;
            Fire(bulletIndex);
        }
    }
}
