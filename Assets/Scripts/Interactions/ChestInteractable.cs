﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ChestInteractable : Interactable
{
    public InventoryItems Contents;
    public InventoryItems RequiredItem;
    public Checkpoints RelatedCheckpoint;

    private Animator animator;
    private bool chestUnlocked;
    private bool contentsTaken;

    public override string description => "open chest";

    public override bool CanPlayerInteract(CreatureController player)
    {
        if (!chestUnlocked)
        {
            if (RequiredItem == InventoryItems.None)
                return true;
            
            var inventory = player.GetComponent<InventoryManager>().Inventory;
            var hasRequiredItem = RequiredItem == InventoryItems.None || inventory.Contains(RequiredItem);
            return hasRequiredItem;
        }

        return !contentsTaken;
    }

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

    void Update()
    {
        if (chestUnlocked)
        {
            return;
        }

        if (RequiredItem != InventoryItems.None && RequiredItem != InventoryItems.Remote)
        {
            var inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();

            if (inventoryManager.Inventory.Contains(RequiredItem))
            {
                UnlockChest();
            }
        }
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (!chestUnlocked)
        {
            this.UnlockChest();
            return;
        }

        if (!contentsTaken)
        {
            if (RequiredItem != InventoryItems.None && RequiredItem != InventoryItems.Remote)
            {
                ev.player.GetComponent<InventoryManager>().RemoveItem(this.RequiredItem);
            }

            OpenChest();
            ev.player.GetComponent<InventoryManager>().GetItem(Contents);
            base.OnInteract(ref ev);
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
