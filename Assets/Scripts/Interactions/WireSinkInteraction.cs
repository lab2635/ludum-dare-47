using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSinkInteraction : Interactable
{
    public int slot;
    public AudioClip ConnectSFX;

    private AudioSource audioSource;
    
    public override string description => "connect cable";

    protected override void OnStart()
    {
        base.OnStart();

        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.ConnectSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
    }

    public void Reset()
    {
        Interact(gameObject);
    }

    public override bool CanPlayerInteract(CreatureController player)
    {
        var attachment = player.attachment;

        if (attachment == null)
            return base.CanPlayerInteract(player);
        
        var source = attachment.GetComponent<WireSourceInteraction>();
        return base.CanPlayerInteract(player) && source != null && source.slot == slot;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        this.audioSource.Play();

        var attachment = ev.player.attachment;

        if (attachment == null)
            return;
        
        var source = attachment.GetComponent<WireSourceInteraction>();
        var cable = attachment.GetComponent<CableProceduralSimple>();
        
        if (source != null && slot == source.slot)
        {
            source.sink = this;
            ev.player.attachment = null;
            cable.endPointTransform = transform;
        }
    }
}
