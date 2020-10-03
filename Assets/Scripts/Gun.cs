using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Bullet[] bullets;
    private float lastTriggerAccumulator = 0f;
    private int bulletIndex;
    private bool firing = false;
    private Collider parentCollider;
    
    private const string PlayerTag = "Player";
    
    public bool gunEnabled = true;
    public bool autoFire = true;
    public float bulletSpeed = 11f;
    public float bulletCount = 1;
    public float fireRate = 0.05f;
    public float triggerRate = 1f;
    public Bullet bulletPrefab;

    public void Hit(Bullet bullet, Collider collision)
    {
        Return(bullet);
        
        if (collision.CompareTag(PlayerTag))
        {
            GameManager.Instance.KillRespawnPlayer();
        }
    }

    void Return(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    
    void Start()
    {
        bullets = new Bullet[20];
        lastTriggerAccumulator = triggerRate;
        parentCollider = GetComponentInParent<Collider>();
        
        for (var i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab);
            bullets[i].source = this;
            bullets[i].speed = bulletSpeed;
            bullets[i].gameObject.SetActive(false);
        }

        GameManager.OnReset += ResetState;
    }

    private IEnumerator Fire()
    {
        firing = true;

        for (var i = 0; i < bulletCount; i++)
        {
            var bullet = bullets[bulletIndex];
            var bulletCollider = bullet.gameObject.GetComponent<Collider>();
            var sourceTransform = bullet.source.gameObject.transform;
            
            if (parentCollider != null)
                Physics.IgnoreCollision(bulletCollider, parentCollider, true);
            
            bullet.transform.rotation = sourceTransform.rotation;
            bullet.transform.position = sourceTransform.position;
            bullet.gameObject.SetActive(true);
            
            bulletIndex = (bulletIndex + 1) % bullets.Length;
            lastTriggerAccumulator = 0;
            
            yield return new WaitForSeconds(fireRate);
        }

        firing = false;
    }
    
    private bool CanTrigger() => gunEnabled && !firing && (lastTriggerAccumulator >= triggerRate);
    
    public void Trigger()
    {
        if (CanTrigger())
        {
            StartCoroutine(Fire());
        }
    }
    
    void Update()
    {
        lastTriggerAccumulator += Time.deltaTime;

        if (autoFire)
        {
            Trigger();
        }
    }

    private void ResetState()
    {
        this.bulletIndex = 0;
        for (var i = 0; i < bullets.Length; i++)
        {
            bullets[i].gameObject.SetActive(false);
        }
    }
}
