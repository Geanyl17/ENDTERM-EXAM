using UnityEngine;

public class ShakeNote : MonoBehaviour
{
    public float shrinkSpeed = 1f;
    public float minSize = 0.2f;
    private bool shakeRegistered = false;

    private void Start()
    {
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
        if (!shakeRegistered && transform.localScale.x > minSize)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
        }
    }

    void HandleShake()
    {
        if (transform.localScale.x <= minSize && !shakeRegistered)
        {
            Debug.Log("✅ Shake success!");
            shakeRegistered = true;
            GameManager.Instance.RegisterHit(); // or bonus
            Destroy(gameObject);
        }
    }
}
