using UnityEngine;
using System;

namespace GameTech
{
    public class ShakeNote : MonoBehaviour
    {
        public float duration = 1.0f;
        public float shrinkSpeed = 0.5f;
        public float targetSize = 0.5f;
        public float missSize = 0.1f;
        public float shakeThreshold = 2.5f;
        
        private bool shakeRegistered = false;
        private bool canShake = false;
        private float startTime;
        private bool hapticTriggered = false;
        private bool hasScored = false;
        
        public event Action OnNoteCompleted;

        private void Start()
        {
            transform.localScale = Vector3.one * 2.0f;
            startTime = Time.time;
            
            // Enable gyro if available
            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
            }
        }

        private void Update()
        {
            float elapsedTime = Time.time - startTime;
            
            // Check if note has expired
            if (elapsedTime >= duration)
            {
                if (!shakeRegistered)
                {
                    HandleMiss();
                }
                return;
            }

            // Calculate size based on elapsed time
            float size = Mathf.Lerp(2.0f, targetSize, elapsedTime / duration);
            transform.localScale = new Vector3(size, size, 1f);

            // Check for scoring when note is at optimal size
            if (!hasScored && elapsedTime >= duration * 0.5f)
            {
                hasScored = true;
                GameManager.Instance.RegisterHit();
            }

            if (!shakeRegistered)
            {
                // Check if we can shake now
                if (!canShake && transform.localScale.x <= targetSize)
                {
                    canShake = true;
                    Handheld.Vibrate();
                }
            }

            // Check for shake input
            if (Input.acceleration.magnitude > shakeThreshold)
            {
                HandleShake();
            }
        }

        /* void TriggerHapticFeedback()
        {
            if (!hapticTriggered)
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        {
                            using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                            {
                                vibrator.Call("vibrate", 100);
                            }
                        }
                    }
                #elif UNITY_IOS && !UNITY_EDITOR
                    Handheld.Vibrate();
                #endif
                hapticTriggered = true;
            }
        } */

        void HandleShake()
        {
            if (canShake && !shakeRegistered)
            {
                shakeRegistered = true;
                OnNoteCompleted?.Invoke();
                Destroy(gameObject);
            }
        }

        private void HandleMiss()
        {
            if (!shakeRegistered)
            {
                GameManager.Instance.RegisterMiss();
                OnNoteCompleted?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
