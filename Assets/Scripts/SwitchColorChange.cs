using UnityEngine;

public class SwitchColorChange : MonoBehaviour
{
    // Assign your button's specific plain material in the Inspector
    public Material buttonMaterial;

    [Header("Color Settings")]
    public Color defaultColor = Color.red;

    [Header("Emission Settings")]
    [ColorUsage(true, true)] // Allows HDR (High Dynamic Range) for glowing colors
    public Color emissionColor = new Color(1f, 0f, 0f, 1f);

    private MaterialPropertyBlock propBlock;
    private Renderer buttonRenderer;

    // Use these internal shader property names (URP compatible)
    private static readonly int BaseColorProperty = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        buttonRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        // Set default state to red at runtime
        TurnOnRed();
    }

    public void TurnOnRed()
    {
        SetButtonColorAndEmission(defaultColor, Color.black); // No emission on default
    }

    public void TurnOnEmission()
    {
        // Applies the glowing emission color
        SetButtonColorAndEmission(defaultColor, emissionColor);
    }

    public void SetButtonColorAndEmission(Color baseColor, Color targetEmission)
    {
        if (buttonRenderer == null) return;

        // Fetch current properties from the renderer
        buttonRenderer.GetPropertyBlock(propBlock);

        // Modify the color and emission properties
        propBlock.SetColor(BaseColorProperty, baseColor);
        propBlock.SetColor(EmissionColorProperty, targetEmission);

        // Apply back to the renderer
        buttonRenderer.SetPropertyBlock(propBlock);
    }
}
