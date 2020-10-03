using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject[] bullets;
    
    [SerializeField]
    public GameObject bulletPrefab;
    
    void Start()
    {
        bullets = new GameObject[20];
        
        for (var i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
