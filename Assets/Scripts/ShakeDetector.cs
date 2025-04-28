using UnityEngine;
using System.Collections;

public class ShakeDetector : MonoBehaviour
{
    public float maxAcceleration = 0.8f;  // Detect when acceleration is below this
    public float minDrop = 0.2f;          // AND drop is above this
    public float minShakeInterval = 0.5f;
    public float checkInterval = 0.1f;    // Only check every 0.1 seconds instead of every frame
    private float lastShakeTime;
    private float lastCheckTime;
    private Vector3 lastAcceleration;
    private float lowPassFilterFactor = 0.1f; // Reduced smoothing for less sensitivity
    private float gravity = 1.0f;  // Normal gravity value when phone is still

    public event System.Action OnShakeDetected;

    void Start()
    {
        lastAcceleration = Input.acceleration;
        lastCheckTime = Time.time;
    }

    void Update()
    {
        // Only check for shakes at the specified interval
        if (Time.time - lastCheckTime < checkInterval)
            return;

        lastCheckTime = Time.time;

        // Skip if we're in cooldown
        if (Time.time - lastShakeTime < minShakeInterval)
            return;

        // Get current acceleration
        Vector3 currentAcceleration = Input.acceleration;
        
        // Apply low-pass filter to smooth the values
        currentAcceleration = Vector3.Lerp(lastAcceleration, currentAcceleration, lowPassFilterFactor);
        
        // Calculate magnitude of acceleration
        float accelerationMagnitude = currentAcceleration.magnitude;
        
        // Calculate how much the acceleration has dropped from normal gravity
        float dropFromGravity = gravity - accelerationMagnitude;
        
        // Debug values
       // Debug.Log($"Acceleration: {accelerationMagnitude:F2}, Drop: {dropFromGravity:F2}, MaxAccel: {maxAcceleration:F2}, MinDrop: {minDrop:F2}");

        if (accelerationMagnitude <= maxAcceleration && dropFromGravity >= minDrop)
        {
          lastShakeTime = Time.time;
           OnShakeDetected?.Invoke();
        }

        lastAcceleration = currentAcceleration;
    }
}
