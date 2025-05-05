using UnityEngine;

namespace GameTech
{
    public class NoteParticleEffect : MonoBehaviour
    {
        public float lifetime = 1.0f;
        public bool autoDestroy = true;
        
        public ParticleSystem particleSystem;
        
        void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
            if (particleSystem == null)
            {
                Debug.LogWarning("ParticleSystem component not found on " + gameObject.name);
            }
        }
        
        void Start()
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
                
                if (autoDestroy)
                {
                    // Make sure we destroy the particle after it completes
                    float totalDuration = particleSystem.main.duration + particleSystem.main.startLifetimeMultiplier;
                    Destroy(gameObject, totalDuration);
                }
                else
                {
                    Destroy(gameObject, lifetime);
                }
            }
        }
    }
}