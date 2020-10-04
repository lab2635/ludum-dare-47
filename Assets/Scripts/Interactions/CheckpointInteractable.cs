using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : Interactable
{
    public bool ActivateCheckpoint;
    public Checkpoints CheckpointToActivate;
    public InventoryItems requiredItem;
    public string activationText = "activate checkpoint";

    public override string description => activationText ?? string.Empty;

    public override bool CanPlayerInteract(CreatureController player)
    {
        if (requiredItem == InventoryItems.None)
            return base.CanPlayerInteract(player);

        var inventory = player.GetComponent<InventoryManager>();
        var hasRequiredItem = inventory.Inventory.Contains(requiredItem);
        var notAlreadyActivated = !GameManager.Instance.HasCheckpoint(CheckpointToActivate);

        return notAlreadyActivated && hasRequiredItem && base.CanPlayerInteract(player);
    }
    
    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (ActivateCheckpoint)
        {
            GameManager.Instance.ActivateCheckpoint(this.CheckpointToActivate);
        }
        
        base.OnInteract(ref ev);
    }
}
