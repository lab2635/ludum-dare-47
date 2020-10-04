﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : Interactable
{
    public InventoryItems RequiredItem;
    public Checkpoints RelatedCheckpoint;

    private Animator animator;
    private bool doorOpened;

    // Start is called before the first frame update
    void Start()
    {
        this.doorOpened = false;
        this.animator = this.gameObject.GetComponent<Animator>();
        GameManager.OnReset += ResetState;

        if (GameManager.Instance.CheckpointList[(int)this.RelatedCheckpoint])
        {
            this.OpenDoor();
        }
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (ev.player.GetComponent<InventoryManager>().Inventory.Contains(this.RequiredItem)
            || this.RequiredItem == InventoryItems.Remote && ev.proxied)
        {
            base.OnInteract(ref ev);

            if (!this.doorOpened)
            {
                this.OpenDoor();
                ev.player.GetComponent<InventoryManager>().RemoveItem(this.RequiredItem);
            }
            else
            {
                this.CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        this.doorOpened = true;
        this.animator.SetTrigger("OpenDoor");
    }

    public void CloseDoor()
    {
        this.doorOpened = false;
        this.animator.SetTrigger("CloseDoor");
    }

    private void ResetState()
    {
        if (this.doorOpened && !GameManager.Instance.CheckpointList[(int)this.RelatedCheckpoint])
        {
            this.CloseDoor();
        }
    }
}
