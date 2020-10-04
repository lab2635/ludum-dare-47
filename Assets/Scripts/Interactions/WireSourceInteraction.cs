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

    protected override void OnStart()
    {
        base.OnStart();

        lineRenderer = GetComponent<LineRenderer>();
        cableMaterial = Instantiate(lineRenderer.material);
        cableMaterial.SetColor("_Color", color);
        lineRenderer.material = cableMaterial;
        
        cable = gameObject.GetComponent<CableProceduralSimple>();
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (!grabbed)
        {
            grabbed = false;
            ev.player.attachment = gameObject;
            cable.endPointTransform = ev.player.handTransform;
        }
    }
}
