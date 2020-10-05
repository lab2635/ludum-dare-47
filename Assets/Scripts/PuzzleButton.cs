using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(MeshRenderer))]
public class PuzzleButton : Interactable
{
    private MeshRenderer meshRenderer;
    private bool active = false;

    private Color originalColor;
    public Color color;
    
    public override string description => !active ? "activate button" : string.Empty;
    public override bool CanPlayerInteract(CreatureController player) => !active;

    protected override void OnStart()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = Instantiate(meshRenderer.material);
        originalColor = meshRenderer.material.color;
        Reset();
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        active = true;
        meshRenderer.material.color = color;
        base.OnInteract(ref ev);
    }

    public void Reset()
    {
        active = false;
    }
}
