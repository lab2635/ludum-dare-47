using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

public class InteractableDetector : MonoBehaviour
{
    public CreatureController player;

    private Interactable previousInteractable;
    private UIView interactionUI;
	private List<Interactable> interactables;

    private void Start()
    {
        this.interactionUI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UIView>();
		interactables = new List<Interactable>();
    }

    void Update()
    {
        // TODO: use input mapping

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var interactable in interactables)
            {
                if (interactable != null && interactable.CanPlayerInteract(player))
                {
                    var interactionEvent = new InteractionEvent
                    {
                        player = player,
                        proxied = false,
                        sender = null,
                        timestamp = Time.time
                    };

                    interactable.Interact(ref interactionEvent);

                    SetInteractionText(interactable.description);
                } 
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var interactable = other.gameObject.GetComponent<Interactable>();
        
        if (interactable != null && interactable.CanPlayerInteract(player))
        {
            interactables.Add(interactable);
            interactable.SetSelecting(true);
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     var interactable = other.gameObject.GetComponent<Interactable>();
    //     
    //     if (interactable != null && interactable.CanInteract(player))
    //     {
    //         SetInteractionText(interactable.description);    
    //     }
    // }

    void OnTriggerExit(Collider other)
    {
        SetInteractionText(string.Empty);
        
        var interactable = other.gameObject.GetComponent<Interactable>();
        
        if (interactable != null)
        {
            if (interactables.Remove(interactable) && interactables.Count > 0)
            {
                var lastInteractable = interactables[interactables.Count - 1];
                SetInteractionText(lastInteractable.description);
            }
        }
    }

    private void SetInteractionText(string text)
    {
        if (text == string.Empty)
        {
            this.interactionUI.Hide();
        }
        else
        {
            this.interactionUI.Show();
            this.interactionUI.GetComponentInChildren<TMPro.TMP_Text>().text = $"Press E to {text}";
        }
    }
}
