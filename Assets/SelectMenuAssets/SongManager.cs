using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongManager : MonoBehaviour
{

    public SongDatabase songDB;

    public TextMeshProUGUI nameText;
    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;

    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }


        UpdateSong(selectedOption);

    }
    public void NextOption()
    {
        selectedOption++;

        if (selectedOption >= songDB.SongCount)
        {
            selectedOption = 0;
        }
        UpdateSong(selectedOption);
        Save();
    }
    public void BackOption()
    {
        selectedOption--;

        if (selectedOption < 0)
        {
            selectedOption = songDB.SongCount -1;
        }
        UpdateSong(selectedOption);
        Save();
    }

    private void UpdateSong(int selecterOption)
    {
        Song song = songDB.GetSong(selectedOption);
        artworkSprite.sprite = song.SongSpritePlaceholder;
        nameText.text = song.SongName;
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }
    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}