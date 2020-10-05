using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class TriggerSwitch : Interactable
{
    public bool enabled;
    public bool triggered;
    public bool autoReset;
    public bool playerActivated;

    protected override void OnStart()
    {
        base.OnStart();

        GameManager.OnReset += this.ResetSelf;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanTriggerWith(other))
        {
            Trigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoReset && CanTriggerWith(other))
        {
            Reset();
        }
    }
    
    private bool CanTrigger => enabled && !triggered;

    private bool CanTriggerWith(Collider other)
    {
        var isPushable = other.CompareTag("Pushable");
        var isPlayerTriggered = other.CompareTag("Player") && playerActivated;
        return isPushable || isPlayerTriggered;
    }
    
    public void Trigger()
    {
        if (CanTrigger)
        {
            triggered = true;
            Interact(gameObject);
        }
    }

    public void Reset()
    {
        triggered = false;
        Interact(gameObject);
    }

    public void ResetSelf()
    {
        triggered = false;
    }
}
