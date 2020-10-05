using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushInteraction : Interactable
{
    private bool pushing = false;
    private Rigidbody body;
    private Vector3 direction;
    private Vector3 target;

    public float distance = 2f;
    public float speed = 5f;

    public AudioClip PushSFX;
    private AudioSource audioSource;

    private Vector3 startPosition;

    public override string description => pushing ? string.Empty : "push box";

    public override bool CanPlayerInteract(CreatureController player) => !pushing;

    protected override void OnStart()
    {
        body = GetComponent<Rigidbody>();
        this.audioSource = gameObject.AddComponent<AudioSource>();
        this.audioSource.clip = this.PushSFX;
        this.audioSource.playOnAwake = false;
        this.audioSource.loop = false;
        this.startPosition = this.transform.position;

        GameManager.OnReset += this.ResetPosition;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        var playerPosition = ev.player.transform.position;
        var pushDirection = transform.position - playerPosition;
        Push(pushDirection.normalized);
    }

    private Vector3 SnapToCardinalDirection(Vector3 v)
    {
        // return the vector with the largest cardinal component (preserving the sign)
        var x = Mathf.Abs(v.x);
        var z = Mathf.Abs(v.z);
        return x > z
            ? new Vector3(Mathf.Sign(v.x), 0, 0)
            : new Vector3(0, 0, Mathf.Sign(v.z));
    }

    private void Push(Vector3 dir)
    {
        if (!pushing)
        {
            this.audioSource.Play();
            pushing = true;
            direction = SnapToCardinalDirection(dir);
            target = body.position + (direction * distance);
        }
    }

    private void FixedUpdate()
    {
        if (!pushing)
        {
            return;
        }
        
        if (body.SweepTest(direction, out RaycastHit hit, speed * (distance / 2) * Time.deltaTime))
        {
            if (!hit.collider.CompareTag("Pushable"))
            {
                pushing = false;
                return;
            }
        }

        var d = Vector3.Distance(body.position, target);
        
        if (d > 0.5)
        {
            body.MovePosition(body.position + direction * speed * Time.deltaTime);
        }
        else
        {
            pushing = false;
        }
    }

    private void ResetPosition()
    {
        this.transform.position = this.startPosition;
    }
}
