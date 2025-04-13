using UnityEngine;
using System.IO;

public class NoteSpawner : MonoBehaviour
{
    public GameObject circleNotePrefab;
    public GameObject squareNotePrefab;
    public GameObject triangleNotePrefab;
    public GameObject shakeNotePrefab;

   // public AudioSource music;
    private BeatmapNote[] notes;
    private int nextNoteIndex = 0;

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "beatmap.json");
        string json = File.ReadAllText(path);
        notes = JsonHelper.FromJson<BeatmapNote>(json);

       // music.Play();
    }

    void Update()
    {
        if (notes == null || nextNoteIndex >= notes.Length) return;

       // if (music.time >= notes[nextNoteIndex].time)
        
            SpawnNote(notes[nextNoteIndex]);
            nextNoteIndex++;
        
    }

    void SpawnNote(BeatmapNote note)
    {
        GameObject prefab = null;

        switch (note.type.ToLower())
        {
            case "circle": prefab = circleNotePrefab; break;
            case "square": prefab = squareNotePrefab; break;
            case "triangle": prefab = triangleNotePrefab; break;
            case "shake": prefab = shakeNotePrefab; break;
        }

        if (prefab != null)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
