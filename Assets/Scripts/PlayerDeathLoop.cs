using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDeathLoop : MonoBehaviour
{
    public GameObject DetonatorPrefab;

    private GameObject body;
    private GameObject respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        this.body = this.gameObject.GetComponentInChildren<Animator>().gameObject;
        this.respawnPoint = GameObject.FindGameObjectWithTag("RespawnPoint");
    }

    public void KillPlayer()
    {
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
