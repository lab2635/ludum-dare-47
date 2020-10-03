using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Gun source;
    public GameObject DetonatorPrefab;

    public float speed = 11f;

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
        Detonator dTemp = (Detonator)this.DetonatorPrefab.GetComponent("Detonator");

        GameObject exp = (GameObject)Instantiate(this.DetonatorPrefab, this.transform.position, Quaternion.identity);
        dTemp = (Detonator)exp.GetComponent("Detonator");
        dTemp.detail = 1.0f;

        Destroy(exp, 10);
    }
}
