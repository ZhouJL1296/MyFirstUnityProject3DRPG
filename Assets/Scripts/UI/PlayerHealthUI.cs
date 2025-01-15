using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    TextMeshProUGUI levelText;
    Image healthSlider;
    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        levelText.text = "Level: " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentHealth / GameManager.Instance.playerStats.characterData.maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerStats.characterData.currentExp / GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
