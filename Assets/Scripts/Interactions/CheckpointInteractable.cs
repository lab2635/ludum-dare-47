using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : Interactable
{
    public bool ActivateCheckpoint;
    public Checkpoints CheckpointToActivate;
    public InventoryItems requiredItem;
    public string activationText = "activate checkpoint";
    public GameObject TrophyObject;

    public override string description => activationText ?? string.Empty;

    protected override void OnStart()
    {
        base.OnStart();

        if(this.TrophyObject != null)
        {
            this.TrophyObject.SetActive(false);
        }
    }

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

            if (this.TrophyObject != null)
            {
                this.TrophyObject.SetActive(true);
            }
        }

        ev.player.GetComponent<InventoryManager>().RemoveItem(requiredItem);

        base.OnInteract(ref ev);
    }
}
