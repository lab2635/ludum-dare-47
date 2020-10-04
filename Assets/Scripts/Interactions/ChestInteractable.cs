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
    private bool chestUnlocked;
    private bool contentsTaken;

    // Start is called before the first frame update
    protected override void OnStart()
    {
        base.OnStart();
        this.animator = this.gameObject.GetComponent<Animator>();
        GameManager.OnReset += ResetState;

        if(this.RequiredItem == InventoryItems.None)
        {
            this.UnlockChest();
        }
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (this.RequiredItem == InventoryItems.Remote)
        {
            if (this.chestUnlocked && !this.contentsTaken)
            {
                this.OpenChest();
                ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
                base.canInteract = false;
            }
            else if (!this.chestUnlocked && !this.contentsTaken && ev.proxied)
            {
                base.OnInteract(ref ev);
                this.UnlockChest();
            }
        }
        else if(!this.contentsTaken && 
            (this.RequiredItem == InventoryItems.None || ev.player.GetComponent<InventoryManager>().Inventory.Contains(this.RequiredItem)))
        {
            this.OpenChest();
            ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
            base.canInteract = false;
        }
    }

    public void UnlockChest()
    {
        this.chestUnlocked = true;
        this.animator.SetTrigger("UnlockChest");
    }

    public void OpenChest()
    {
        this.contentsTaken = true;
        this.animator.SetTrigger("OpenChest");
    }

    public void CloseChest()
    {
        this.chestUnlocked = false;
        this.animator.SetTrigger("CloseChest");
    }

    private void ResetState()
    {
        if (!GameManager.Instance.CheckpointList[(int)this.RelatedCheckpoint])
        {
            if (this.chestUnlocked && this.RequiredItem != InventoryItems.None)
            {
                this.CloseChest();
            }
            else if(this.contentsTaken && this.RequiredItem == InventoryItems.None)
            {
                this.UnlockChest();
            }
            this.contentsTaken = false;
            base.canInteract = true;
        }
    }
}
