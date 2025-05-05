using UnityEngine;
using System;
using System.Collections;

public class ShakeDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Max allowed acceleration magnitude to trigger shake")]
    [Range(0.1f, 1.5f)] public float maxAcceleration = 0.8f;
    
    [Tooltip("Minimum drop from normal gravity to trigger")]
    [Range(0.1f, 0.5f)] public float minDrop = 0.2f;
    
    [Tooltip("Minimum time between shake detections (seconds)")]
    [Range(0.1f, 1f)] public float minShakeInterval = 0.5f;
    
    [Header("Advanced")]
    [Tooltip("How often to check for shakes (seconds)")]
    [Range(0.02f, 0.2f)] public float checkInterval = 0.1f;
    
    [Tooltip("Smoothing factor (lower = smoother)")]
    [Range(0.05f, 0.3f)] public float lowPassFilterFactor = 0.1f;
    
    [Tooltip("Expected gravity magnitude when device is still")]
    [Range(0.8f, 1.2f)] public float expectedGravity = 1.0f;

    // Debugging
    public bool showDebug = false;
    
    private float _lastShakeTime;
    private float _lastCheckTime;
    private Vector3 _lastAcceleration;
    private Vector3 _lowPassValue;
    
    public event Action OnShakeDetected;

    void Start()
    {
        _lastAcceleration = Input.acceleration;
        _lowPassValue = _lastAcceleration;
        _lastCheckTime = Time.time;
        
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        float currentTime = Time.time;
        
        // Throttle checks to reduce CPU usage
        if (currentTime - _lastCheckTime < checkInterval)
            return;
            
        _lastCheckTime = currentTime;

        // Skip if in cooldown
        if (currentTime - _lastShakeTime < minShakeInterval)
            return;

        // Get and filter acceleration
        Vector3 currentAcceleration = Input.acceleration;
        _lowPassValue = Vector3.Lerp(_lowPassValue, currentAcceleration, lowPassFilterFactor);
        
        // Calculate magnitude and drop
        float accelMagnitude = _lowPassValue.magnitude;
        float dropFromGravity = expectedGravity - accelMagnitude;

        /*if (showDebug)
        {
            Debug.Log($"Accel: {accelMagnitude:F2} | " +
                     $"Drop: {dropFromGravity:F2} | " +
                     $"Filtered: {_lowPassValue}");
        }*/

        // Detection condition
        if (accelMagnitude <= maxAcceleration && dropFromGravity >= minDrop)
        {
            _lastShakeTime = currentTime;
            OnShakeDetected?.Invoke();
        }

        _lastAcceleration = currentAcceleration;
    }

    // Helper to adjust detection for current device
    public void CalibrateForCurrentDevice()
    {
        // Sample current device gravity over 1 second
        StartCoroutine(CalibrationRoutine());
    }

    private IEnumerator CalibrationRoutine()
    {
        float calibrationDuration = 1f;
        float endTime = Time.time + calibrationDuration;
        int samples = 0;
        Vector3 totalAcceleration = Vector3.zero;

        while (Time.time < endTime)
        {
            totalAcceleration += Input.acceleration;
            samples++;
            yield return new WaitForSeconds(0.1f);
        }

        if (samples > 0)
        {
            expectedGravity = (totalAcceleration / samples).magnitude;
            if (showDebug) Debug.Log($"Calibrated gravity: {expectedGravity:F2}");
        }
    }
}