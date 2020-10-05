using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    public GameObject DetonatorPrefab;
    public AudioClip DeathSFX;

    private GameObject body;
    private GameObject respawnPoint;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
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

        this.body.SetActive(false);
    }

    public IEnumerator RespawnPlayer(System.Action action)
    {
        yield return new WaitForSeconds(2);
        action.Invoke();
        this.transform.position = this.respawnPoint.transform.position;
        this.body.SetActive(true);
    }
}
