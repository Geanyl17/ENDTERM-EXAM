using UnityEngine;

namespace GameTech
{
    public class MusicFade : MonoBehaviour
    {
        public float fadeSpeed = 1f; // Speed of the fade
        public float minAlpha = 0f; // Minimum alpha value
        public float maxAlpha = 1f; // Maximum alpha value
        public float pulseDuration = 0.2f; // Duration of each pulse
        public float cycleDuration = 0.5f; // Duration of complete cycle

        private SpriteRenderer spriteRenderer;
        private float pulseTimer = 0f;
        private Color originalColor;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogWarning("No SpriteRenderer found on this object!");
                return;
            }
            originalColor = spriteRenderer.color;
        }

        private void Update()
        {
            if (spriteRenderer == null) return;

            pulseTimer += Time.deltaTime;
            float targetAlpha = pulseTimer < pulseDuration ? maxAlpha : minAlpha;
            
            // Smoothly interpolate the alpha
            Color currentColor = spriteRenderer.color;
            currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed * 20f);
            spriteRenderer.color = currentColor;

            if (pulseTimer >= cycleDuration)
            {
                pulseTimer = 0f;
            }
        }
    }
} 