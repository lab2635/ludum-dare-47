using UnityEngine;

public class WirePuzzle : Interactable
{
    public WireSourceInteraction[] wires;

    private float checkAccumulator;
    private bool solved;

    protected override void OnStart()
    {
        GameManager.OnReset += Reset;
        base.OnStart();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<CreatureController>();
            
            for (var i = 0; i < wires.Length; i++)
            {
                if (wires[i].gameObject == player.attachment)
                {
                    wires[i].Reset();
                    player.RemoveAttachment();
                    break;
                }
            }
        }
    }
    
    public void Reset()
    {
        solved = false;
        
        for (var i = 0; i < wires.Length; i++)
        {
            wires[i].Reset();
        }
    }
    
    void Update()
    {
        checkAccumulator += Time.deltaTime;

        if (checkAccumulator >= 0.5f)
        {
            checkAccumulator = 0;
            
            if (!solved && Solved())
            {
                Debug.Log("SOLVED");
                solved = true;
                Interact(gameObject);
            }
        }
    }

    bool Solved()
    {
        for (var i = 0; i < wires.Length; i++)
        {
            if (!wires[i].sink)
                return false;
        }

        return true;
    }
}
