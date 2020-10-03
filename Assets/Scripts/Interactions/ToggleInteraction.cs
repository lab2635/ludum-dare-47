public abstract class ToggleInteraction : Interactable
{
    public string objectName = "object";
    public string activeTemplate = "activate {0}";
    public string inactiveTemplate = "deactivate {0}";
    public bool toggled = false;

    private string activeDescription;
    private string inactiveDescription;

    public override string description => toggled
        ? inactiveDescription
        : activeDescription;

    protected override void OnStart()
    {
        activeDescription = string.Format(activeTemplate ?? string.Empty, objectName);
        inactiveDescription = string.Format(inactiveTemplate ?? string.Empty, objectName);
    }

    protected override void OnInteract(ref InteractionEvent ev)
    {
         toggled = !toggled;
    }
}

