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
                if (interactable != null && Input.GetKeyDown(KeyCode.E))
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
        
        if (interactable != null)
        {
            if (CanInteract(interactable) && interactable.CanInteract(player))
            {
                interactables.Add(interactable);
                SetInteractionText(interactable.description);
                interactable.SetSelecting(true);
            }
        }
    }

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
    
    private bool CanInteract(Interactable interactable)
    {
        if (!interactable.CanInteract(player))
        {
            return false;
        }

        return true;
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
