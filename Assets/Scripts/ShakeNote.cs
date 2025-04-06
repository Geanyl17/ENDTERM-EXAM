using UnityEngine;

public class ShakeNote : MonoBehaviour
{
    public float shrinkSpeed = 1f;
    public float minSize = 0.2f;
    public float shakeThreshold = 2.5f;

    private bool canShake = false;
    private bool success = false;

    void Start()
    {
        Input.gyro.enabled = true; // optional if using gyro
    }

    void Update()
    {
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
