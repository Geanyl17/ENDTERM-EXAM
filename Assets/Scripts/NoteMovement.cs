using UnityEngine;

namespace GameTech
{
    public class NoteMovement : MonoBehaviour
    {
        public Vector3 targetPosition;
        public float speed;
        public float rotationSpeed = 180f; // Degrees per second
        public float pulseSpeed = 2f; // Pulses per second
        public float pulseAmount = 0.1f; // How much to scale up/down

        private Vector3 originalScale;
        private float pulseTime;
        private bool hasScored = false;

        void Start()
        {
            originalScale = transform.localScale;
            pulseTime = 0f;
        }

        void Update()
        {
            // Move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Rotate the note
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Pulse effect
            pulseTime += Time.deltaTime;
            float pulse = Mathf.Sin(pulseTime * pulseSpeed * Mathf.PI * 2) * pulseAmount;
            transform.localScale = originalScale * (1f + pulse);

            // Check if note is close to center for scoring
            if (!hasScored && Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                hasScored = true;
                GameManager.Instance.RegisterHit();
            }

            // Destroy when reaching target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                if (!hasScored)
                {
                    GameManager.Instance.RegisterMiss();
                }
                Destroy(gameObject);
            }
        }
    }
} 