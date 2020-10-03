using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestInteractable : Interactable
{
    public InventoryItems Contents;

    private Animator animator;
    private bool chestOpened;
    private bool contentsTaken;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = this.gameObject.GetComponent<Animator>();
        GameManager.OnReset += ResetState;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (this.chestOpened && !this.contentsTaken)
        {
            this.contentsTaken = true;
            ev.player.GetComponent<InventoryManager>().GetItem(this.Contents);
            base.canInteract = false;
        }
        else if(!this.chestOpened && !this.contentsTaken && ev.proxied)
        { 
            base.OnInteract(ref ev);
            this.chestOpened = true;
            this.OpenChest();
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
        this.chestOpened = false;
        this.contentsTaken = false;
        base.canInteract = true;
        this.CloseChest();
        // would like to set interactable message here
    }
}
