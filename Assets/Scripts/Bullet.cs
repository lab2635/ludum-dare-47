using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Turret source;

    [SerializeField] public float speed = 11f;
    
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        source.Hit(this, other);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
