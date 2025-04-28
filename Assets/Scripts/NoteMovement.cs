using UnityEngine;
using GameTech;

namespace GameTech
{
    public class NoteMovement : MonoBehaviour
    {
        public Vector3 targetPosition;
        public float speed = 2.0f;
        public float rotationSpeed = 180f; // Degrees per second
        public float pulseSpeed = 2f; // Pulses per second (2 = one complete cycle per second)
        public float pulseAmount = 0.2f; // How much to add to default size

        private NoteSpawner noteSpawner;
        private float pulseTimer = 0f;
        private bool hasScored = false;
        private Vector3 originalScale;

        private void Start()
        {
            noteSpawner = FindFirstObjectByType<NoteSpawner>();
            if (noteSpawner == null)
            {
                Debug.LogWarning("NoteSpawner not found in scene!");
            }
            originalScale = transform.localScale;
        }

        private void Update()
        {
            // Move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Rotate the note
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Pulse effect
            pulseTimer += Time.deltaTime;
            float targetScale = pulseTimer < 0.2f ? pulseAmount : 0f;
            float currentScale = Mathf.Lerp(transform.localScale.x - originalScale.x, targetScale, Time.deltaTime * 20f);
            transform.localScale = originalScale + new Vector3(currentScale, currentScale, currentScale);
            
            if (pulseTimer >= 0.5f)
            {
                pulseTimer = 0f;
            }

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
                if (noteSpawner != null)
                {
                    noteSpawner.RemoveActiveNote(gameObject);
                }
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // Ensure the note is removed from the active list when destroyed
            if (noteSpawner != null)
            {
                noteSpawner.RemoveActiveNote(gameObject);
            }
        }
    }
} 