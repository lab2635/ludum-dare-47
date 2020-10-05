using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreenInteraction : Interactable
{
    public GameObject screen;
    public bool hideOnReset = false;
    public Checkpoints RelatedCheckpoint;

    protected override void OnStart()
    {
        GameManager.OnReset += Reset;
        base.OnStart();
    }

    void Reset()
    {
        if (hideOnReset)
        {
            screen.SetActive(false);
        }

        if(RelatedCheckpoint == Checkpoints.GunRoomComplete && !GameManager.Instance.CheckpointList[(int)RelatedCheckpoint])
        {
            screen.SetActive(false);
        }
    }
    
    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        screen.SetActive(!screen.activeSelf);
    }
}
