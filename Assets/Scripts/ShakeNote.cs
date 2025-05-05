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
        private ShakeDetector shakeDetector;

        
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

            shakeDetector = FindFirstObjectByType<ShakeDetector>();
            if (shakeDetector != null)
            {
                shakeDetector.OnShakeDetected += HandleShake;
            }
        }

        private void Update()
        {
            float elapsedTime = Time.time - startTime;
            
            // Calculate size (shrinking from 2.0 → 0.5)
            float size = Mathf.Lerp(2.0f, targetSize, elapsedTime / duration);
            transform.localScale = new Vector3(size, size, 1f);

            // NEW: Allow shaking when scale <= 1.0 (instead of waiting for targetSize)
            if (!canShake && size <= 1.0f)
            {
                canShake = true;
                if (!hapticTriggered)
                {
                    TriggerHapticFeedback();
                    hapticTriggered = true;
                }
            }

            // Debug log to track scale and shake state
            Debug.Log($"Scale: {size:F2} | CanShake: {canShake} | ShakeRegistered: {shakeRegistered}");

            // Check for miss if note expires without shaking
            if (elapsedTime >= duration && !shakeRegistered)
            {
                HandleMiss();
            }
        }

            void TriggerHapticFeedback()
            {
                if (hapticTriggered) return;

            #if UNITY_ANDROID && !UNITY_EDITOR
                try
                {
                    // Get Android vibration service with proper error handling
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                    {
                        // Check if vibration is supported
                        bool hasVibrator = vibrator.Call<bool>("hasVibrator");
                        
                        if (hasVibrator)
                        {
                            // Standard vibration pattern (300ms)
                            long[] pattern = { 0, 300 }; // Immediate start, 300ms duration
                            vibrator.Call("vibrate", pattern, -1); // -1 = no repeat
                            
                            Debug.Log("Android vibration triggered successfully");
                        }
                        else
                        {
                            Debug.LogWarning("Vibration not available on this device");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Vibration error: " + e.Message);
                }
            #elif UNITY_IOS && !UNITY_EDITOR
                // iOS uses a more pronounced vibration pattern
                if (iOSHapticFeedback.IsSupported())
                {
                    iOSHapticFeedback.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
                }
                else
                {
                    Handheld.Vibrate();
                }
                Debug.Log("iOS vibration triggered");
            #endif

                hapticTriggered = true;
            }
            
        void HandleShake()
        {
            if (canShake && !shakeRegistered)
            {
                shakeRegistered = true;
                TriggerHapticFeedback();
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

        private void OnDestroy()
        {
            if (shakeDetector != null)
            {
                shakeDetector.OnShakeDetected -= HandleShake;
            }
        }

    }
}