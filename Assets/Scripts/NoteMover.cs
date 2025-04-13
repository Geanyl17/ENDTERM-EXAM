using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("HitArea").transform; // Ensure your center has this tag
    }

    void Update()
    {
        if (target)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
