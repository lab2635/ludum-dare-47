using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSinkInteraction : Interactable
{
    protected override void OnInteract(ref InteractionEvent ev)
    {
        var attachment = ev.player.attachment;
        var source = attachment.GetComponent<WireSourceInteraction>();
        var cable = attachment.GetComponent<CableProceduralSimple>();
        
        if (source != null)
        {
            ev.player.attachment = null;
            cable.endPointTransform = transform;
        }
    }
}
