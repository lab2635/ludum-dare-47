﻿using UnityEngine;

public struct InteractionEvent
{
    public Interactable sender;
    public bool proxied;
    public float timestamp;
    public CreatureController player;
}

public abstract class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public float cooldown = 0f;
    public Interactable[] proxies;
    public float holdTime;

    private float nextInteraction;

    public virtual string description => string.Empty;

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() { }

    protected virtual void OnInteract(ref InteractionEvent ev)
    {
        if (proxies != null)
        {
            foreach (var proxy in proxies)
            {
                var proxyEvent = new InteractionEvent
                {
                    proxied = true,
                    sender = this,
                    player = ev.player,
                    timestamp = ev.timestamp
                };

                proxy.Interact(ref proxyEvent);
            }
        }
    }

    public void SetSelecting(bool value)
    {
        foreach (var child in GetComponentsInChildren<Transform>(true))
        {
            if (value)
            {
                child.gameObject.layer |= 8;
            }
            else
            {
                child.gameObject.layer &= ~8;
            }
        }
    }

    public virtual bool CanPlayerInteract(CreatureController player)
    {
        return canInteract && Time.time >= nextInteraction;
    }

    public void Interact(GameObject sender)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CreatureController>();
        var ev = new InteractionEvent
        {    
            timestamp = Time.time,
            proxied = true,
            player = player,
            sender = sender.GetComponent<Interactable>()
        };
        
        Interact(ref ev);
    }
    
    public void Interact(ref InteractionEvent ev)
    {
        OnInteract(ref ev);

        nextInteraction = Time.time + cooldown;
    }
}

