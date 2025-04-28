using UnityEngine;
using UnityEngine.SceneManagement;

public class retryscript : MonoBehaviour
{
    public void RetryLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
