using UnityEngine;

public class WirePuzzle : Interactable
{
    public WireSourceInteraction[] wires;
    public bool playerInsideBounds;
    
    private float checkAccumulator;
    private bool solved;

    protected override void OnStart()
    {
        GameManager.OnReset += Reset;
        SetWiresEnabled(false);
        base.OnStart();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetWiresEnabled(true);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetWiresEnabled(false);
            
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
            wires[i].canInteract = false;
            wires[i].Reset();
        }
    }
    
    void SetWiresEnabled(bool value)
    {
        for (var i = 0; i < wires.Length; i++)
        {
            wires[i].canInteract = value;
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
