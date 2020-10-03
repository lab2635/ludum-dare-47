using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Gun source;
    
    public float speed = 11f;
    
    private void OnTriggerEnter(Collider other)
    {
        source.Hit(this, other);
    }
    
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }


}
