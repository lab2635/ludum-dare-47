using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteraction : ToggleInteraction
{
    public bool OnlyShootable;
    public bool ActivateCheckpoint;
    public Checkpoints CheckpointToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (this.OnlyShootable && other.gameObject.CompareTag("PlayerBullet"))
        {
            var interactionEvent = new InteractionEvent
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<CreatureController>(),
                proxied = true,
                sender = other.gameObject.GetComponent<Interactable>(),
                timestamp = Time.time
            };

            base.Interact(ref interactionEvent);
        }
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);

        if (this.ActivateCheckpoint)
        {
            GameManager.Instance.ActivateCheckpoint(this.CheckpointToActivate);
        }
    }
}
