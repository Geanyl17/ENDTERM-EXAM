using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameTech
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int score = 0;
        public int combo = 0;

        [SerializeField] private NumberDisplay scoreDisplay;
        [SerializeField] private ComboDisplay comboDisplay;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        void Start()
        {
            // Initialize displays
            UpdateScoreDisplay();
            UpdateComboDisplay();
        }

        public void RegisterHit()
        {
            combo++;
            score += 100 * combo;
            Debug.Log($"✅ HIT | Score: {score} | Combo: {combo}");
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
