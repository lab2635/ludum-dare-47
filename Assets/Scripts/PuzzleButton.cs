using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(MeshRenderer))]
public class PuzzleButton : Interactable
{
    private MeshRenderer meshRenderer;
    private Material buttonMaterial;
    private bool active = false;

    public Color color;
    
    public override string description => !active ? "activate button" : string.Empty;
    public override bool CanPlayerInteract(CreatureController player) => !active;

    protected override void OnStart()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        buttonMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = buttonMaterial;
        Reset();
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        active = true;
        buttonMaterial.color = Color.white;
    }

    public void Reset()
    {
        active = false;
        
        if (buttonMaterial != null)
        {
            buttonMaterial.color = color;
        }
    }
}
