using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestInteractable : ToggleInteraction
{
    public InventoryItems Contents;
    public InventoryItems RequiredItem;
    public Checkpoints RelatedCheckpoint;

    private Animator animator;
    private bool chestOpened;
    private bool contentsTaken;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
        this.animator = this.gameObject.GetComponent<Animator>();
        GameManager.OnReset += ResetState;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (this.RequiredItem == InventoryItems.Remote)
        {
            if (this.chestOpened && !this.contentsTaken)
            {
                this.contentsTaken = true;
                ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
                base.canInteract = false;
            }
            else if (!this.chestOpened && !this.contentsTaken && ev.proxied)
            {
                base.OnInteract(ref ev);
                this.OpenChest();
            }
        }
        else if(!this.contentsTaken && 
            (this.RequiredItem == InventoryItems.None || ev.player.GetComponent<InventoryManager>().Inventory.Contains(this.RequiredItem)))
        {
            this.OpenChest();
            ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
            this.contentsTaken = true;
            base.canInteract = false;
        }
    }

    public void OpenChest()
    {
        this.chestOpened = true;
        this.animator.SetTrigger("OpenChest");
    }

    public void CloseChest()
    {
        this.chestOpened = false;
        this.animator.SetTrigger("CloseChest");
    }

    private void ResetState()
    {
        if (!GameManager.Instance.CheckpointList[(int)this.RelatedCheckpoint])
        {
            if (this.chestOpened)
            {
                this.CloseChest();
            }
            this.contentsTaken = false;
            base.canInteract = true;
        }
    }
}
