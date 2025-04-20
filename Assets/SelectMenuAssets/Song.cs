using UnityEngine;
using TMPro; // Required for TextMeshPro

[System.Serializable]
public class Song
{
    public string SongName;
    public Sprite SongSpritePlaceholder;
    public TextMeshProUGUI SongNameTextMeshPro; // Assign this in the Inspector
}