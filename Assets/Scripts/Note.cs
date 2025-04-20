using UnityEngine;

namespace GameTech
{
    public class Note : MonoBehaviour
    {
        public HitAreaShape noteShape;
        public float moveSpeed = 5f;

        private Transform target;

        void Start()
        {
            // Look for the HitArea GameObject by tag
            GameObject hitArea = GameObject.FindGameObjectWithTag("HitArea");
            if (hitArea != null)
            {
                target = hitArea.transform;
            }
            else
            {
                Debug.LogWarning("HitArea not found! Make sure it's tagged 'HitArea'");
            }
        }

        void Update()
        {
            if (target == null) return;

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("HitArea"))
            {
                GameManager.Instance.RegisterMiss(); // Register hit in GameManager
                Debug.Log($"Note {noteShape} reached the HitArea!");
                Destroy(gameObject); // Destroy the note after it reaches the HitArea
            }
        }
    }
}
