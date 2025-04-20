using UnityEngine;
using UnityEngine.UI;

namespace GameTech
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] private Image[] digitImages; // Array of Image components for each digit
        [SerializeField] private Sprite[] numberSprites = new Sprite[10]; // Array of sprites for numbers 0-9
        [SerializeField] private Image xSymbol; // The X symbol image
        [SerializeField] private float symbolSpacing = 3f; // Space between X and last digit

        private void Start()
        {
            // Set preserve aspect for all digit images
            foreach (Image image in digitImages)
            {
                image.preserveAspect = true;
            }
            if (xSymbol != null)
            {
                xSymbol.preserveAspect = true;
            }
            HideAllDigits(); // Start with hidden display
        }

        public void DisplayCombo(int number)
        {
            // If combo is 0, hide all digits and symbol
            if (number == 0)
            {
                HideAllDigits();
                if (xSymbol != null) xSymbol.gameObject.SetActive(false);
                return;
            }

            string numberString = number.ToString();
            int lastVisibleDigitIndex = -1;
            
            // Display each digit
            for (int i = 0; i < digitImages.Length; i++)
            {
                if (i < numberString.Length)
                {
                    // Get the digit and display it
                    int digit = int.Parse(numberString[i].ToString());
                    digitImages[i].sprite = numberSprites[digit];
                    digitImages[i].gameObject.SetActive(true);
                    lastVisibleDigitIndex = i;
                }
                else
                {
                    // Hide unused digits
                    digitImages[i].gameObject.SetActive(false);
                }
            }

            // Position X symbol after the last visible digit
            if (xSymbol != null && lastVisibleDigitIndex >= 0)
            {
                xSymbol.gameObject.SetActive(true);
                RectTransform symbolRect = xSymbol.GetComponent<RectTransform>();
                RectTransform lastDigitRect = digitImages[lastVisibleDigitIndex].GetComponent<RectTransform>();
                
                // Position X after the last digit
                symbolRect.anchoredPosition = new Vector2(
                    lastDigitRect.anchoredPosition.x + (lastDigitRect.sizeDelta.x / 2 + symbolRect.sizeDelta.x / 2 + symbolSpacing),
                    lastDigitRect.anchoredPosition.y
                );
            }
        }

        private void HideAllDigits()
        {
            foreach (Image image in digitImages)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
} 