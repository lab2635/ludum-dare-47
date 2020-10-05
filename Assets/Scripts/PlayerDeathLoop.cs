using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    public GameObject DetonatorPrefab;
    public AudioClip DeathSFX;

    private CreatureController player;

    private GameObject body;
    private GameObject respawnPoint;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CreatureController>();
        this.body = this.gameObject.GetComponentsInChildren<Animator>()[1].gameObject;
        this.respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.DeathSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
        this.audioSource.volume = 0.5f;
    }

    public void KillPlayer()
    {
        this.audioSource.Play();

        Detonator dTemp = (Detonator)this.DetonatorPrefab.GetComponent("Detonator");

        GameObject exp = (GameObject)Instantiate(this.DetonatorPrefab, this.transform.position, Quaternion.identity);
        dTemp = (Detonator)exp.GetComponent("Detonator");
        dTemp.detail = 1.0f;

        Destroy(exp, 2);

        player.gameObject.SetActive(false);
        this.body.SetActive(false);
    }

    public void RespawnPlayer()
    {
        this.transform.position = this.respawnPoint.transform.position;
        this.body.SetActive(true);

        player.gameObject.SetActive(true);
        player.transform.position = respawnPoint.transform.position;
    }
}
