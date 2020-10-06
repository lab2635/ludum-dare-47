using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    public GameObject DetonatorPrefab;
    public AudioClip DeathSFX;
    
    private GameObject body;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // this.body = this.gameObject.GetComponentsInChildren<Animator>()[1].gameObject;
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.DeathSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
        this.audioSource.volume = 0.5f;
    }

    public void KillPlayer(CreatureController player)
    {
        this.audioSource.Play();

        Detonator dTemp = (Detonator)this.DetonatorPrefab.GetComponent("Detonator");
        
        GameObject exp = (GameObject)Instantiate(DetonatorPrefab, player.transform.position, Quaternion.identity);
        dTemp = (Detonator)exp.GetComponent("Detonator");
        dTemp.detail = 1.0f;

        Destroy(exp, 2);
        
        player.gameObject.SetActive(false);
        // body.SetActive(false);
    }

    public void RespawnPlayer(CreatureController player)
    {
        var respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");

        // body.SetActive(true);
        player.transform.position = respawnPoint.transform.position;
        player.gameObject.SetActive(true);
    }
}
