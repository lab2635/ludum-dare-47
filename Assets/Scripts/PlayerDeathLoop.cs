using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    private GameObject body;
    private GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        this.body = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
        this.respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
    }

    public async Task KillPlayer()
    {
        // explosion or something
        this.body.SetActive(false);
    }

    public async Task RespawnPlayer()
    {
        this.transform.position = this.respawnPoint.transform.position;
        this.body.SetActive(true);
        await Task.Delay(100);
    }
}
