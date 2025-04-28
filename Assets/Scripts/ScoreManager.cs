using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameTech;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public string date;

    public LeaderboardEntry(string name, int score)
    {
        this.playerName = name;
        this.score = score;
        this.date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    }
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static GameManager instance { get; private set; }
    private int currentScore = 0;
    private LeaderboardData leaderboardData;
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize leaderboard
        LoadLeaderboard();

        // Add some test data if leaderboard is empty
        if (leaderboardData.entries.Count == 0)
        {
            leaderboardData.entries.Add(new LeaderboardEntry("TestPlayer1", 1000));
            leaderboardData.entries.Add(new LeaderboardEntry("TestPlayer2", 1500));
            leaderboardData.entries.Add(new LeaderboardEntry("TestPlayer3", 1200));

            // Sort the entries (optional)
            leaderboardData.entries = leaderboardData.entries.OrderByDescending(x => x.score).ToList();

            // Save the leaderboard with test data
            SaveLeaderboard();
        }
    }


    private void InitializeLeaderboard()
    {
        savePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        LoadLeaderboard();
    }

    public void AddScore(int points)
    {
        currentScore += points;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

    public void SaveScore(string playerName)
    {
        int scoreToSave = GameManager.Instance.score;  // Get the score directly from GameManager

        if (scoreToSave > 0)
        {
            leaderboardData.entries.Add(new LeaderboardEntry(playerName, scoreToSave));
            leaderboardData.entries = leaderboardData.entries.OrderByDescending(x => x.score).ToList();

            // Keep only top 10 scores
            if (leaderboardData.entries.Count > 10)
            {
                leaderboardData.entries = leaderboardData.entries.Take(10).ToList();
            }

            SaveLeaderboard();

            // Debug: Log the leaderboard to ensure it's being saved correctly
            Debug.Log("Leaderboard after saving score:");
            foreach (var entry in leaderboardData.entries)
            {
                Debug.Log(entry.playerName + ": " + entry.score);
            }
        }
    }



    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboardData.entries;
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardData);
        Debug.Log("Saving leaderboard to " + savePath);  // Debugging the save path
        File.WriteAllText(savePath, json);
    }


    private void LoadLeaderboard()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            leaderboardData = JsonUtility.FromJson<LeaderboardData>(json);
            Debug.Log("Leaderboard loaded successfully.");
        }
        else
        {
            leaderboardData = new LeaderboardData();
            Debug.Log("No leaderboard file found, creating a new one.");
        }
    }

} 