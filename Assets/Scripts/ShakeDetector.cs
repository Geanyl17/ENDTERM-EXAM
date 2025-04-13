using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 2.5f; // Tweak this value depending on device
    public float cooldown = 0.5f;       // To avoid detecting multiple shakes too fast

    private float lastShakeTime = 0f;

    public System.Action OnShakeDetected;

    void Update()
    {
        if (Time.time - lastShakeTime > cooldown)
        {
            float acceleration = Input.acceleration.magnitude;

            if (acceleration > shakeThreshold)
            {
                lastShakeTime = Time.time;
                Debug.Log("📱 Shake detected!");

                // Call any assigned function
                OnShakeDetected?.Invoke();
            }
        }
    }
}
