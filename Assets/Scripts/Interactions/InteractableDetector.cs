using System;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;

[RequireComponent(typeof(CreatureController))]
public class InteractableDetector : MonoBehaviour
{
    public LayerMask layer;
    public float detectionRadius = 2f;

    private Collider[] interactionColliders;
    private CreatureController player;
    private Interactable previousInteractable;
    private UIView interactionUI;

    private void OnDrawGizmos()
    {
        if (player != null) 
        {
            Gizmos.color = Color.red;
            var offset = Vector3.up * detectionRadius;
            Gizmos.DrawSphere(player.transform.position + offset, detectionRadius);
        }
    }

    private void Start()
    {
        player = GetComponent<CreatureController>();
        interactionColliders = new Collider[3];
        interactionUI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UIView>();
    }

    void Update()
    {
        var activationKeyDown = Input.GetKeyDown(KeyCode.E);
        var offset = Vector3.up * detectionRadius;
        var count = Physics.OverlapSphereNonAlloc(player.transform.position + offset, detectionRadius, interactionColliders, layer);

        if (count < 1)
        {
            SetInteractionText(string.Empty);
            return;
        }

        for (var i = 0; i < count; i++)
        {
            var interactionCollider = interactionColliders[i];
            var interactable = interactionCollider.GetComponent<Interactable>();

            if (interactable != null && interactable.CanPlayerInteract(player))
            {
                SetInteractionText(interactable.description);

                if (activationKeyDown && GameManager.Instance.IsPlayerControllerEnabled)
                {
                    interactable.Interact(player);
                }
            }
        }
    }
    
    // private void OnTriggerStay(Collider other)
    // {
    //     var interactable = other.gameObject.GetComponent<Interactable>();
    //     
    //     if (interactable != null && interactable.CanPlayerInteract(player))
    //     {
    //         interactable.SetSelecting(true);
    //         SetInteractionText(interactable.description);
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     var interactable = other.gameObject.GetComponent<Interactable>();
    //
    //     if (interactable != null)
    //     {
    //         interactable.SetSelecting(false);
    //         SetInteractionText(string.Empty);
    //     }
    // }

    // void OnTriggerEnter(Collider other)
    // {
    //     var interactable = other.gameObject.GetComponent<Interactable>();
    //     
    //     if (interactable != null && interactable.CanPlayerInteract(player))
    //     {
    //         interactables.Add(interactable);    
    //         interactable.SetSelecting(true);
    //         SetInteractionText(interactable.description);
    //     }
    // }

    // private void OnTriggerStay(Collider other)
    // {
    //     
    //     if (interactables.Count > 0)
    //     {
    //         var lastInteractable = interactables[interactables.Count - 1];
    //         
    //         if (lastInteractable.CanPlayerInteract(player))
    //         {
    //             SetInteractionText(lastInteractable.description);
    //         }
    //         else
    //         {
    //             SetInteractionText(string.Empty);
    //         }
    //     }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     SetInteractionText(string.Empty);
    //     
    //     var interactable = other.gameObject.GetComponent<Interactable>();
    //     
    //     if (interactable != null)
    //     {
    //         if (interactables.Remove(interactable) && interactables.Count > 0)
    //         {
    //             var lastInteractable = interactables[interactables.Count - 1];
    //             SetInteractionText(lastInteractable.description);
    //         }
    //     }
    // }

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
