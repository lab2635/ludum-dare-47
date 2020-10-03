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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            Trigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (autoReset)
        {
            Reset();
        }
    }

    private bool CanTrigger => enabled && !triggered;
    
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
}
