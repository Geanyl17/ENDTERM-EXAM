using UnityEngine;

public class BeatmapManager : MonoBehaviour
{
    public BeatmapNote[] beatmapNotes;

    void Start()
    {
        LoadBeatmap("beatmap");
    }

    void LoadBeatmap(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null)
        {
            beatmapNotes = JsonHelper.FromJson<BeatmapNote>(jsonFile.text);
            Debug.Log("Beatmap loaded with " + beatmapNotes.Length + " notes.");
        }
        else
        {
            Debug.LogError("Beatmap file not found: " + fileName);
        }
    }
}

