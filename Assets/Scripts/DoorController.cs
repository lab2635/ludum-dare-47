using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorController : ToggleInteraction
{
    private Animator animator;
    private bool doorOpened;

    // Start is called before the first frame update
    void Start()
    {
        this.doorOpened = false;
        this.animator = this.gameObject.GetComponent<Animator>();
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        this.doorOpened = !this.doorOpened;

        if (this.doorOpened)
        {
            this.OpenDoor();
        }
        else
        {
            this.CloseDoor();
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
}
