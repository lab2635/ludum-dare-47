using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckpointInteractable))]
public class SwitchInteraction : MonoBehaviour
{
    public bool OnlyShootable;
    public Animator ButtonAnimator;
    public AudioClip PushSFX;

    private AudioSource audioSource;
    private CheckpointInteractable interactable;

    private void Start()
    {
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.PushSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.OnlyShootable && other.gameObject.CompareTag("PlayerBullet"))
        {
            if (!this.ButtonAnimator.GetBool("isPressed"))
            {
                this.ButtonAnimator.SetBool("isPressed", true);
                this.audioSource.Play();

                var interactionEvent = new InteractionEvent
                {
                    player = GameObject.FindGameObjectWithTag("Player").GetComponent<CreatureController>(),
                    proxied = true,
                    sender = other.gameObject.GetComponent<Interactable>(),
                    timestamp = Time.time
                };

                this.interactable = this.gameObject.GetComponent<CheckpointInteractable>();
                this.interactable.Interact(ref interactionEvent);

                this.interactable.canInteract = false;
            }
        }
    }
}
