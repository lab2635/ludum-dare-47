using System.Collections.Generic;
using UnityEngine;

public class OrderPuzzleInteraction : Interactable
{
    public PuzzleButton[] buttons;
    public bool debug = false;
    
    private Stack<PuzzleButton> state;
    private Material indicatorMaterial;
    
    protected override void OnStart()
    {
        state = new Stack<PuzzleButton>();
        // var meshRenderer = GetComponent<MeshRenderer>();
        // indicatorMaterial = Instantiate(meshRenderer.material);
        // meshRenderer.material = indicatorMaterial;
        ResetPuzzle();
        base.OnStart();
    }

    private bool HasWon => state.Count == 0;
    
    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (HasWon)
        {
            if (debug) ResetPuzzle();
            base.OnInteract(ref ev);
            return;
        }
        
        var button = ev.sender as PuzzleButton;

        if (state.Peek() == button)
        {
            state.Pop();
            
            if (state.Count == 0)
            {
                Won();
            }
        }
        else
        {
            Lost();
        }
    }

    private void Won()
    {
        // indicatorMaterial.color = Color.yellow;
        Interact(gameObject);
    }

    private void Lost()
    {
        // indicatorMaterial.color = Color.white;
        ResetPuzzle();
    }

    private void ResetPuzzle()
    {
        // indicatorMaterial.color = Color.white;
        state.Clear();
        
        for (var i = buttons.Length - 1; i >= 0; i--)
        {
            var button = buttons[i];
            button.Reset();
            state.Push(button);
        }   
    }
}
