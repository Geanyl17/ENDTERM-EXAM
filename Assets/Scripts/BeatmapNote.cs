[System.Serializable]
public class BeatmapNote
{
    public float time;
    public string type;
    public float duration = 1.0f; // Default duration for shake notes
    public float angle; // Angle in degrees (0-360)
}