using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestInteractable : ToggleInteraction
{
    public InventoryItems Contents;
    public bool OpenRemotely;

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
        if (this.OpenRemotely)
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
                this.chestOpened = true;
                this.OpenChest();
            }
        }
        else if(!this.contentsTaken)
        {
            this.OpenChest();
            this.chestOpened = true;
            ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
            this.contentsTaken = true;
            base.canInteract = false;
        }
    }

    public void OpenChest()
    {
        this.animator.SetTrigger("OpenChest");
    }

    public void CloseChest()
    {
        this.animator.SetTrigger("CloseChest");
    }

    private void ResetState()
    {
        if (this.chestOpened)
        {
            this.CloseChest();
        }
        this.chestOpened = false;
        this.contentsTaken = false;
        base.canInteract = true;
        // would like to set interactable message here
    }
}
