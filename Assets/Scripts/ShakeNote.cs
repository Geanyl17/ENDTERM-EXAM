using UnityEngine;
using System;

public class ShakeNote : MonoBehaviour
{
    public float shrinkSpeed = 0.5f;
    private bool shakeRegistered = false;
    private bool canShake = false;
    private float targetSize = 0.5f; // Circle(1)'s size
    private float missSize = 0.1f;   // Size where it counts as a miss
    
    public event Action OnNoteCompleted;

    private void Start()
    {
        // Start at size 2.0
        transform.localScale = Vector3.one * 2.0f;
        Debug.Log($"Note spawned at size: {transform.localScale.x:F2}");
        
        GameObject detector = GameObject.Find("ShakeDetector");
        if (detector != null)
        {
            ShakeDetector shake = detector.GetComponent<ShakeDetector>();
            if (shake != null)
                shake.OnShakeDetected += HandleShake;
        }
    }

    void Update()
    {
        if (!shakeRegistered)
        {
            if (transform.localScale.x > missSize)
            {
                transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
                
                // Debug current size every frame
                Debug.Log($"Current size: {transform.localScale.x:F2}, Target size: {targetSize:F2}, CanShake: {canShake}");
                
                // Check if we've reached the size where shaking can begin
                if (!canShake && transform.localScale.x <= targetSize)
                {
                    canShake = true;
                    Debug.Log($"Can shake now! Current size: {transform.localScale.x:F2}");
                }
            }
            else
            {
                // Note has shrunk too much without being shaken
                HandleMiss();
            }
        }
    }

    void HandleShake()
    {
        if (!shakeRegistered)
        {
            Debug.Log($"Shake detected! Size: {transform.localScale.x:F2}, CanShake: {canShake}, TargetSize: {targetSize:F2}");
            
            // Check if size is between targetSize and missSize
            if (transform.localScale.x <= targetSize && transform.localScale.x > missSize)
            {
                Debug.Log($"✅ Shake success! Size: {transform.localScale.x:F2}");
                shakeRegistered = true;
                GameManager.Instance.RegisterHit();
                OnNoteCompleted?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"❌ Shake failed! Note size: {transform.localScale.x:F2}, needs to be between {missSize:F2} and {targetSize:F2}");
                HandleMiss();
            }
        }
    }

    void HandleMiss()
    {
        if (!shakeRegistered)
        {
            Debug.Log($"❌ Shake missed! Final size: {transform.localScale.x:F2}");
            GameManager.Instance.RegisterMiss();
            OnNoteCompleted?.Invoke();
            Destroy(gameObject);
        }
    }
}
