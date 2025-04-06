using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int combo = 0;

    public ComboUI comboUI; // ← reference to UI script

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterHit()
    {
        combo++;
        score += 100 * combo;
        Debug.Log($"✅ HIT | Score: {score} | Combo: {combo}");
        comboUI.UpdateCombo(combo);
    }

    public void RegisterMiss()
    {
        combo = 0;
        Debug.Log("❌ MISS | Combo reset.");
        comboUI.UpdateCombo(combo);
    }
}
