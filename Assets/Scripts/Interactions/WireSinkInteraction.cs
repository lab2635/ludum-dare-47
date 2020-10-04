﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSinkInteraction : Interactable
{
    public int slot;

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
