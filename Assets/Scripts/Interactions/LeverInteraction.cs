using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeverInteraction : ToggleInteraction
{
    public LeverController controller;

    public override string description => toggled ? "deactivate" : "activate";

    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        Toggle(toggled);
    }

    public void Toggle(bool value)
    {
        toggled = value;
        controller.angle = value ? controller.maxAngle : controller.minAngle;
    }
}

