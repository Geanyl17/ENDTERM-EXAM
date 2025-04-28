using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GameTech;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI maxComboText;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform leaderboardContent;
    [SerializeField] private GameObject leaderboardEntryPrefab;

    [Header("Scene Names")]
    [SerializeField] private string retrySceneName;
    [SerializeField] private string mainMenuSceneName;

    private void Start()
    {
        playerNameInput.Select();
        playerNameInput.ActivateInputField();
        totalScoreText.text = $"Total Score: {GameManager.Instance.score}";  // Show score from GameManager
        leaderboardPanel.SetActive(false);
        maxComboText.text = $"Max Combo: {GameManager.Instance.GetMaxCombo()}"; // Show max combo from GameManager

    }

    public void SaveScore()
    {
        string playerName = string.IsNullOrEmpty(playerNameInput.text) ? "Anonymous" : playerNameInput.text;

        // Debugging if SaveScore method is being called
        Debug.Log("SaveScore triggered. Player: " + playerName);

        // Pass the score from GameManager to ScoreManager for saving
        ScoreManager.Instance.SaveScore(playerName);
        DisplayLeaderboard();
        TouchScreenKeyboard.hideInput = true;
    }


    public void RetryLevel()
    {
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(retrySceneName);
    }

    public void ReturnToMainMenu()
    {
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        DisplayLeaderboard();
    }

    private void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }

    private void DisplayLeaderboard()
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        List<LeaderboardEntry> entries = ScoreManager.Instance.GetLeaderboard();
        int rank = 1;
        foreach (LeaderboardEntry entry in entries)
        {
            GameObject obj = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            obj.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rank.ToString();
            obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = entry.playerName;
            obj.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();
            rank++;
        }
    }


}
