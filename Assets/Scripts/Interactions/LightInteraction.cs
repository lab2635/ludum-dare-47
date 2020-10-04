using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightInteraction : ToggleInteraction
{
    public Color bulbColor = Color.white;
    public float intensity = 1f;
    public float lightIntensityMultiplier = 10f;
    public float responseTime = 25;
    public bool inheritIntesity = true;
    public bool on = true;
    public bool pulse = false;
    public float pulsesPerSecond = 0f;

    private Light attachedLight;
    private MeshRenderer bulbRenderer;
    private Material bulbMaterial;
    private Color currentColor;
    private float currentIntensity = 0f;
    
    protected override void OnInteract(ref InteractionEvent ev)
    {
        base.OnInteract(ref ev);
        on = toggled;
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        bulbRenderer = GetComponent<MeshRenderer>();
        bulbMaterial = new Material(bulbRenderer.sharedMaterial);
        bulbMaterial.color = bulbColor;
        bulbRenderer.material = bulbMaterial;
        
        attachedLight = gameObject.GetComponent<Light>();

        if (attachedLight == null)
        {
            attachedLight = GetComponentInChildren<Light>(true);
        }

        if (attachedLight != null && inheritIntesity)
        {
            intensity = attachedLight.intensity;
        }
    }

    public void Update()
    {
        var targetIntensity = on ? intensity : 0f;

        if (pulse)
        {
            var frequency = 1f / pulsesPerSecond;
            var alpha = 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * frequency * Time.time));
            targetIntensity = alpha * intensity;
        }

        currentColor = bulbColor;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * responseTime);

        if (bulbMaterial != null)
        {
            bulbMaterial.color = currentColor;
            bulbMaterial.SetColor("_EmissionColor", currentColor * currentIntensity);
        }

        if (attachedLight != null)
        {
            attachedLight.color = currentColor;
            attachedLight.intensity = currentIntensity * lightIntensityMultiplier;
        }
    }

    public void Toggle(bool value)
    {
        toggled = value;
        on = value;
    }
}
