using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScreenInteraction : Interactable
{
    public GameObject screen;
    
    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        screen.SetActive(!screen.activeSelf);
    }
}
