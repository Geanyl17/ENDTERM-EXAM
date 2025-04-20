using UnityEngine;

public class ShakeNote : MonoBehaviour
{
<<<<<<< Updated upstream
    public float shrinkSpeed = 1f;
    public float minSize = 0.2f;
    public float shakeThreshold = 2.5f;
=======
    public float shrinkSpeed = 0.5f;
    public float duration = 1.0f;  // How long the note should be active
    private bool shakeRegistered = false;
    private bool canShake = false;
    private float targetSize = 0.5f; // Circle(1)'s size
    private float missSize = 0.1f;   // Size where it counts as a miss
    private float startTime;
    private bool hapticTriggered = false;
    
    public event Action OnNoteCompleted;
>>>>>>> Stashed changes

    private bool canShake = false;
    private bool success = false;

    void Start()
    {
<<<<<<< Updated upstream
        Input.gyro.enabled = true; // optional if using gyro
=======
        // Start at size 2.0
        transform.localScale = Vector3.one * 2.0f;
        startTime = Time.time;
        Debug.Log($"Note spawned at size: {transform.localScale.x:F2}, Duration: {duration:F2}s");
        
        GameObject detector = GameObject.Find("ShakeDetector");
        if (detector != null)
        {
            ShakeDetector shake = detector.GetComponent<ShakeDetector>();
            if (shake != null)
                shake.OnShakeDetected += HandleShake;
        }
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Shrink the circle
        if (transform.localScale.x > minSize)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

            if (transform.localScale.x <= minSize)
            {
                canShake = true;
                Debug.Log("Shake now!");
            }
        }
        else if (canShake && !success)
=======
        if (Time.time - startTime >= duration)
        {
            OnNoteCompleted?.Invoke();
            Destroy(gameObject);
        }

        if (!shakeRegistered)
        {
            // Check if note has expired
            if (Time.time - startTime > duration)
            {
                Debug.Log($"Note expired! Time: {Time.time - startTime:F2}s, Duration: {duration:F2}s");
                HandleMiss();
                return;
            }

            if (transform.localScale.x > missSize)
            {
                // Calculate size based on time elapsed
                float timeElapsed = Time.time - startTime;
                float progress = timeElapsed / duration;
                transform.localScale = Vector3.one * Mathf.Lerp(2.0f, missSize, progress);
                
                // Debug current size every frame
                Debug.Log($"Current size: {transform.localScale.x:F2}, Progress: {progress:F2}, CanShake: {canShake}");
                
                // Check if we've reached the size where shaking can begin
                if (!canShake && transform.localScale.x <= targetSize)
                {
                    canShake = true;
                    Debug.Log($"Can shake now! Current size: {transform.localScale.x:F2}");
                    // Trigger haptic feedback when note is ready to be shaken
                    if (!hapticTriggered)
                    {
                        TriggerHapticFeedback();
                        hapticTriggered = true;
                    }
                }
            }
            else
            {
                // Note has shrunk too much without being shaken
                HandleMiss();
            }
        }
    }

    void TriggerHapticFeedback()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            // For Android devices
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                    {
                        vibrator.Call("vibrate", 100); // Vibrate for 100ms
                    }
                }
            }
        #elif UNITY_IOS && !UNITY_EDITOR
            // For iOS devices
            Handheld.Vibrate();
        #else
            // For testing in editor
            Debug.Log("Haptic feedback would trigger here on mobile device");
        #endif
    }

    void HandleShake()
    {
        if (!shakeRegistered)
>>>>>>> Stashed changes
        {
            // Check for shake input
            if (Input.acceleration.magnitude > shakeThreshold)
            {
                success = true;
                Debug.Log("✅ Shake success!");
                GameManager.Instance.RegisterHit(); // or special bonus
                Destroy(gameObject);
            }
        }
    }
}
