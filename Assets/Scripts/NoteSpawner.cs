using UnityEngine;
using System.IO;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    public GameObject circleNotePrefab;
    public GameObject squareNotePrefab;
    public GameObject triangleNotePrefab;
    public GameObject rectangleNotePrefab;
    public GameObject shakeNotePrefab;

   // public AudioSource music;
    private BeatmapNote[] notes;
    private int nextNoteIndex = 0;
    private bool isShakeNoteActive = false;
    private GameObject currentShakeNote;

    void Start()
    {
        StartCoroutine(LoadBeatmap());
    }

    IEnumerator LoadBeatmap()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "beatmap.json");
        string json;

        if (Application.platform == RuntimePlatform.Android)
        {
            // On Android, use WWW to read from StreamingAssets
            WWW www = new WWW(path);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                json = www.text;
            }
            else
            {
                Debug.LogError("Failed to load beatmap: " + www.error);
                yield break;
            }
        }
        else
        {
            // For other platforms, we can read directly
            json = File.ReadAllText(path);
        }

        notes = JsonHelper.FromJson<BeatmapNote>(json);

       // music.Play();
    }

    private float timer = 0f;

    void Update()
    {
        if (notes == null || nextNoteIndex >= notes.Length) return;

        timer += Time.deltaTime;

        if (timer >= notes[nextNoteIndex].time && !isShakeNoteActive)
        {
            SpawnNote(notes[nextNoteIndex]);
            nextNoteIndex++;
        }
    }

    void SpawnNote(BeatmapNote note)
    {
        GameObject prefab = null;
        Vector3 spawnPosition;

        switch (note.type.ToLower())
        {
            case "circle": prefab = circleNotePrefab; break;
            case "square": prefab = squareNotePrefab; break;
            case "triangle": prefab = triangleNotePrefab; break;
            case "rectangle": prefab = rectangleNotePrefab; break;
            case "shake": 
                prefab = shakeNotePrefab;
                isShakeNoteActive = true;
                break;
        }

        if (prefab != null)
        {
            if (note.type.ToLower() == "shake")
            {
                // Spawn shake notes at the center
                spawnPosition = Vector3.zero;
            }
            else
            {
                // Spawn other notes at random angles
                float angle = Random.Range(0f, 360f);
                float spawnRadius = 8f;
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                spawnPosition = direction * spawnRadius;
            }

            GameObject spawnedNote = Instantiate(prefab, spawnPosition, Quaternion.identity);
            
            if (note.type.ToLower() == "shake")
            {
                currentShakeNote = spawnedNote;
                ShakeNote shakeNote = spawnedNote.GetComponent<ShakeNote>();
                if (shakeNote != null)
                {
                    // Subscribe to the note's completion
                    shakeNote.OnNoteCompleted += HandleShakeNoteCompleted;
                }
            }
        }
    }

    void HandleShakeNoteCompleted()
    {
        isShakeNoteActive = false;
        currentShakeNote = null;
    }
}