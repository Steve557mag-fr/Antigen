using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThermometer : MonoBehaviour
{
    [SerializeField] CanvasGroup group;
    [SerializeField] TextMeshProUGUI indicatorDigital; 
    [SerializeField] Image fill;
    [SerializeField] float fillMinValue = 0.1f;

    public void UpdateUI(float temperature)
    {
        float oldTemperature = fill.fillAmount.Remap(fillMinValue,1, 36f, 38.5f);
        LeanTween.value(oldTemperature,temperature, 1.25f).setOnUpdate((float f) =>
        {
            fill.fillAmount = temperature.Remap(36, 38.5f, fillMinValue, 1);
            indicatorDigital.text = $"{temperature}°C";
        });
    }
}
