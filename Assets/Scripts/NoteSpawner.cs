using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ShapePrefab
    {
        public HitAreaShape shape;
        public GameObject prefab;
    }

    public ShapePrefab[] notePrefabs;
    public float spawnInterval = 1.5f;
    public float spawnRadius = 6f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomNote();
            timer = 0f;
        }
    }

    void SpawnRandomNote()
    {
        int index = Random.Range(0, notePrefabs.Length);
        ShapePrefab selected = notePrefabs[index];

        // Calculate a random direction around the center
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnRadius;

        // Instantiate and assign noteShape
        GameObject noteGO = Instantiate(selected.prefab, spawnPos, Quaternion.identity);
        Note note = noteGO.GetComponent<Note>();
        if (note != null)
        {
            note.noteShape = selected.shape;
        }
    }
}
