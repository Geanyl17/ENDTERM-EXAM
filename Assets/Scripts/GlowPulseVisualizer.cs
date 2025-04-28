using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlowPulseVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public float boost = 5f; // Reduced from 50f to 5f
    public float smoothSpeed = 5f;
    public float scaleMultiplier = 0.5f;
    public Color baseColor = Color.white;
    public Color glowColor = Color.cyan;

    private SpriteRenderer spriteRenderer;
    private float currentIntensity;
    private Vector3 originalScale;
    private float[] samples = new float[256];
    private float maxSampleValue = 0.1f; // Threshold for normalization

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        audioSource.GetOutputData(samples, 0);

        float sum = 0f;
        float maxValue = 0f;

        // Find the maximum value in the samples
        foreach (float s in samples)
        {
            float absValue = Mathf.Abs(s);
            maxValue = Mathf.Max(maxValue, absValue);
            sum += absValue;
        }

        // Normalize the values
        float normalizedValue = maxValue / maxSampleValue;
        float intensity = Mathf.Clamp01(normalizedValue * boost);
        currentIntensity = Mathf.Lerp(currentIntensity, intensity, Time.deltaTime * smoothSpeed);

        // Scale pulse
        transform.localScale = originalScale * (1f + currentIntensity * scaleMultiplier);

        // Glow color shift
        spriteRenderer.color = Color.Lerp(baseColor, glowColor, currentIntensity);
    }
}
