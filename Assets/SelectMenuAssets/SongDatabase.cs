using UnityEngine;

[CreateAssetMenu]
public class SongDatabase : ScriptableObject
{
    public Song[] song;
        public int SongCount
    {
        get
        {
            return song.Length;
        }
    }
    public Song GetSong(int index)
    {
        return song[index];
    }
}
