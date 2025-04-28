using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

namespace GameTech
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int score = 0;
        public int combo = 0;
        public int maxCombo = 0; // ADD THIS 🔥

        private ScoreManager scoreManager;

        [SerializeField] private NumberDisplay scoreDisplay;
        [SerializeField] private ComboDisplay comboDisplay;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            scoreManager = ScoreManager.Instance;
        }

        void Start()
        {
            // Initialize displays
            UpdateScoreDisplay();
            UpdateComboDisplay();
        }

        public int GetMaxCombo()
        {
            return maxCombo;
        }

        public int GetScore()
        {
            return score;
        }
        public void RegisterHit()
        {
            combo++;
            score += 100 * combo;
            
            // 🔥 Update max combo if current combo is higher
            if (combo > maxCombo)
            {
                maxCombo = combo;
                
            }

            Debug.Log($"✅ HIT | Score: {score} | Combo: {combo} | Max Combo: {maxCombo}");
            UpdateScoreDisplay();
            UpdateComboDisplay();
        }

        public void RegisterMiss()
        {
            combo = 0;
            Debug.Log("❌ MISS | Combo reset.");
            UpdateComboDisplay();
        }

        private void UpdateScoreDisplay()
        {
            if (scoreDisplay != null)
            {
                scoreDisplay.DisplayNumber(score);
            }
        }

        private void UpdateComboDisplay()
        {
            if (comboDisplay != null)
            {
                comboDisplay.DisplayCombo(combo);
            }
        }
    }
}
