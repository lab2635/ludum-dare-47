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
    public AudioClip GrabSFX;

    private AudioSource audioSource;
    public Color color;
    public int slot;
    public WireSinkInteraction sink;

    public override string description => grabbed ? string.Empty : "grab cable";

    protected override void OnStart()
    {
        base.OnStart();

        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.GrabSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;

        lineRenderer = GetComponent<LineRenderer>();
        cableMaterial = Instantiate(lineRenderer.material);
        cableMaterial.SetColor("_Color", color);
        lineRenderer.material = cableMaterial;
        
        cable = gameObject.GetComponent<CableProceduralSimple>();
    }

    public void Reset()
    {
        if (sink != null)
            sink.Reset();

        cable.Clear();
        sink = null;
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
            this.audioSource.Play();
            grabbed = true;
            ev.player.attachment = gameObject;
            cable.endPointTransform = ev.player.handTransform;
        }
    }
}
