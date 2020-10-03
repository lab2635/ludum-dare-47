using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : Interactable
{
    public InventoryItems RequiredItem;

    private Animator animator;
    private bool doorOpened;

    // Start is called before the first frame update
    void Start()
    {
        this.doorOpened = false;
        this.animator = this.gameObject.GetComponent<Animator>();
        GameManager.OnReset += ResetState;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (ev.player.GetComponent<InventoryManager>().Inventory.Contains(this.RequiredItem))
        {
            base.OnInteract(ref ev);
            this.doorOpened = !this.doorOpened;

            if (this.doorOpened)
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
        this.animator.SetTrigger("OpenDoor");
    }

    public void CloseDoor()
    {
        this.animator.SetTrigger("CloseDoor");
    }

    private void ResetState()
    {
        if (this.doorOpened)
        {
            this.CloseDoor();
        }
        this.doorOpened = false;
        // would like to set interactable message here
    }
}
