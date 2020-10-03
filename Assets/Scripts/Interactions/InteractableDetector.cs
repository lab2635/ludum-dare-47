using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public CreatureController player;
    public TMPro.TextMeshProUGUI interactionText;

    private Interactable previousInteractable;

    void Update()
    {
        // TODO: use input mapping
        
        if (previousInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            var interactionEvent = new InteractionEvent
            {
                player = player,
                proxied = false,
                sender = previousInteractable,
                timestamp = Time.time
            };
            
            previousInteractable.Interact(ref interactionEvent);
            SetInteractionText(previousInteractable.description);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var interactable = other.gameObject.GetComponent<Interactable>();
        
        if (interactable != null)
        {
            if (CanInteract(interactable) && interactable.CanInteract(player))
            {
                previousInteractable = interactable;
                SetInteractionText(previousInteractable.description);
                interactable.SetSelecting(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        SetInteractionText(string.Empty);
        
        if (previousInteractable != null)
        {
            previousInteractable.SetSelecting(false);
            previousInteractable = null;
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
        if (interactionText != null)
        {
            interactionText.text = text;
        }
    }
}
