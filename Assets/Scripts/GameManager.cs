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
        [SerializeField] private GameObject circleParticle;
        [SerializeField] private GameObject squareParticle; 
        [SerializeField] private GameObject triangleParticle;
        [SerializeField] private GameObject rectangleParticle;

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
        public void RegisterHit(string noteType = null, Vector3 position = default)
        {
            combo++;
            score += 100 * combo;
            
            // 🔥 Update max combo if current combo is higher
            if (combo > maxCombo)
            {
                maxCombo = combo;
                
            }
            
            if (noteType != null && position != default)
            {
                SpawnParticle(noteType, position);
            }

            Debug.Log($"✅ HIT | Score: {score} | Combo: {combo} | Max Combo: {maxCombo}");
            UpdateScoreDisplay();
            UpdateComboDisplay();
        }

        public void SpawnParticle(string noteType, Vector3 position)
        {
            GameObject particlePrefab = null;
            
            switch (noteType.ToLower())
            {
                case "circle":
                    particlePrefab = circleParticle;
                    break;
                case "square":
                    particlePrefab = squareParticle;
                    break;
                case "triangle":
                    particlePrefab = triangleParticle;
                    break;
                case "rectangle":
                    particlePrefab = rectangleParticle;
                    break;
            }
            
            if (particlePrefab != null)
            {
                Instantiate(particlePrefab, position, Quaternion.identity);
            }
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
