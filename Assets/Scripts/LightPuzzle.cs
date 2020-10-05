using System;
using UnityEngine;

public class LightPuzzle : Interactable
{
    public LightInteraction[] lights;
    public LeverInteraction[] levers;

    private int state;

    private const int OFF_STATE = 0b00000000;
    private const int WIN_STATE = 0b00011111;
    private const int LIGHTS = 5;

    private static readonly int[] solutions = new int[]
    {
        0b01101, 0b10110, 0b11100, 0b10001, 0b00100, 0b01010, 0b00111,
        0b11011, 0b01110, 0b01001, 0b10010, 0b11000, 0b10101, 0b00011
    };

    protected override void OnStart()
    {
        state = OFF_STATE;
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
        if (state == OFF_STATE)
        {
            Spawn();
            return;
        }
        
        if (ev.proxied && ev.sender is LeverInteraction lever)
        {
            var index = Array.IndexOf(levers, lever);
            
            if (index >= 0)
            {
                Toggle(index);
                
                if (Synchronize())
                    base.OnInteract(ref ev);
            }
        }
        else
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Generate();
        Synchronize();

        // toggleSound = FMODUnity.RuntimeManager.CreateInstance(toggleEvent);
        // solveSound = FMODUnity.RuntimeManager.CreateInstance(solveEvent);
        // solveSound.setVolume(0.4f);
    }

    private bool Synchronize()
    {
        var won = IsWon();
        var interactable = !won;
        
        for (var i = 0; i < levers.Length; i++)
        {
            var value = (state >> i) & 1;
            levers[i].canInteract = interactable;
            lights[i].canInteract = interactable;
            lights[i].Toggle(value != 0);
            levers[i].Toggle(value != 0);
        }

        return won;
    }


    private bool IsWon()
    {
        return state == WIN_STATE;
    }

    private void Toggle(int index)
    {
        state ^= (1 << index);

        if (index - 1 >= 0) state ^= (1 << index - 1);
        if (index + 1 < LIGHTS) state ^= (1 << index + 1);

        // toggleSound.start();
    }

    private void Generate()
    {
        var index = UnityEngine.Random.Range(0, LIGHTS);
        state = solutions[index];
    }

    // public void Interacted(LightPuzzleInteraction interaction)
    // {
    //     var index = Array.FindIndex(items, x => x == interaction);
    //     
    //     Toggle(index);
    //
    //     if (Synchronize())
    //     {
    //         solveSound.start();
    //         GameManager.Instance.SetPowerActive(true);
    //     }
    // }
}
