using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : Interactable
{
    public bool ActivateCheckpoint;
    public Checkpoints CheckpointToActivate;
    public InventoryItems requiredItem;

    public override bool CanPlayerInteract(CreatureController player)
    {
        if (requiredItem == InventoryItems.None)
            return base.CanPlayerInteract(player);

        var inventory = player.GetComponent<InventoryManager>();
        var hasRequiredItem = inventory.Inventory.Contains(requiredItem);

        return base.CanPlayerInteract(player) && hasRequiredItem;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);

        if (this.ActivateCheckpoint)
        {
            GameManager.Instance.ActivateCheckpoint(this.CheckpointToActivate);
        }
    }
}
