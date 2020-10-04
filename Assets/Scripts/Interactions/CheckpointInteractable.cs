using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteractable : Interactable
{
    public bool ActivateCheckpoint;
    public Checkpoints CheckpointToActivate;

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);

        if (this.ActivateCheckpoint)
        {
            GameManager.Instance.ActivateCheckpoint(this.CheckpointToActivate);
        }
    }
}
