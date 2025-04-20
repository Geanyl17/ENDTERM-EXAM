using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene1 : MonoBehaviour
{
    //for the song choices scene transitions
    public void GoToSong1()
    {
        SceneManager.LoadScene("PlaySceneSong1");
    }
    public void GoToSong2()
    {
        SceneManager.LoadScene("PlaySceneSong2");
    }
    public void GoToSong3()
    {
        SceneManager.LoadScene("PlaySceneComingSoon");
    }


    //for the PLAY scene transitions

    public void SampleScene1()
    {
        SceneManager.LoadScene("SampleScene1");
    }
    public void SampleScene2()
    {
        SceneManager.LoadScene("SampleScene2");
    }

    public void MenuSample()
    {
        SceneManager.LoadScene("Menu");
    }

}
