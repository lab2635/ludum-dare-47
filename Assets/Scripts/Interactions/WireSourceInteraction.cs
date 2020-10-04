using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CableProceduralSimple))]
public class WireSourceInteraction : Interactable
{
    private CableProceduralSimple cable;
    private LineRenderer lineRenderer;
    private Material cableMaterial;
    private bool grabbed = false;
    
    public Color color;
    public int slot;
    public WireSinkInteraction sink;

    protected override void OnStart()
    {
        base.OnStart();

        lineRenderer = GetComponent<LineRenderer>();
        cableMaterial = Instantiate(lineRenderer.material);
        cableMaterial.SetColor("_Color", color);
        lineRenderer.material = cableMaterial;
        
        cable = gameObject.GetComponent<CableProceduralSimple>();
    }

    public void Reset()
    {
        // if (sink != null)
        //     sink.Reset();
        //
        // sink = null;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (ev.player.attachment == gameObject)
        {
            grabbed = false;
            cable.Clear();
            ev.player.attachment = null;
            return;
        }
        
        if (ev.player.attachment == null)
        {
            grabbed = true;
            ev.player.attachment = gameObject;
            cable.endPointTransform = ev.player.handTransform;
        }
    }
}
