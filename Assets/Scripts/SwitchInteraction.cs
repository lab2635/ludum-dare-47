using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteraction : ToggleInteraction
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
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
}
