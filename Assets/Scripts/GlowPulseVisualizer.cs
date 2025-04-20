using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlowPulseVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public float boost = 50f; // Multiplier for the glow
    public float smoothSpeed = 5f;
    public float scaleMultiplier = 0.5f;
    public Color baseColor = Color.white;
    public Color glowColor = Color.cyan;

    private SpriteRenderer spriteRenderer;
    private float currentIntensity;
    private Vector3 originalScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        float[] samples = new float[256];
        audioSource.GetOutputData(samples, 0);

        float sum = 0f;
        foreach (float s in samples)
            sum += s * s;

        float rmsValue = Mathf.Sqrt(sum / samples.Length);
        float intensity = Mathf.Clamp01(rmsValue * boost);
        currentIntensity = Mathf.Lerp(currentIntensity, intensity, Time.deltaTime * smoothSpeed);

        // Scale pulse
        transform.localScale = originalScale * (1f + currentIntensity * scaleMultiplier);

        // Glow color shift
        spriteRenderer.color = Color.Lerp(baseColor, glowColor, currentIntensity);
    }
}
