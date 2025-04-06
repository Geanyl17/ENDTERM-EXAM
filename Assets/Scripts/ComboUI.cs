using TMPro;
using UnityEngine;

public class ComboUI : MonoBehaviour
{
    public TextMeshProUGUI comboText;

    void Start()
    {
        UpdateCombo(0);
    }

    public void UpdateCombo(int combo)
    {
        comboText.text = $"{combo}";
    }
}
