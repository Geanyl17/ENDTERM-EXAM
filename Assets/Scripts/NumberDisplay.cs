using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameTech
{
    public class NumberDisplay : MonoBehaviour
    {
        [SerializeField] private Image[] digitImages; // Array of Image components for each digit
        [SerializeField] private Sprite[] numberSprites = new Sprite[10]; // Array of sprites for numbers 0-9

        private void Start()
        {
            // Set preserve aspect for all digit images
            foreach (Image image in digitImages)
            {
                image.preserveAspect = true;
            }
            DisplayNumber(0); // Show all zeros at start
        }

        public void DisplayNumber(int number)
        {
            // Format number with leading zeros based on number of digit images
            string numberString = number.ToString().PadLeft(digitImages.Length, '0');
            
            // Display each digit
            for (int i = 0; i < digitImages.Length; i++)
            {
                // Get the digit and display it
                int digit = int.Parse(numberString[i].ToString());
                digitImages[i].sprite = numberSprites[digit];
                digitImages[i].gameObject.SetActive(true);
            }
        }
    }
} 