using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CubeInteraction : ToggleInteraction
{
    public GameObject target;

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        
        var renderer = GetComponent<MeshRenderer>();
        
        if (renderer != null)
        {
            Debug.Log("interacted");
            renderer.material.color = toggled ? Color.blue : Color.red;
        }


    }
}

