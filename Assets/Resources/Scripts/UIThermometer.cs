using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThermometer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI indicatorDigital; 
    [SerializeField] Image fill;
    [SerializeField] float fillMinValue = 0.1f;

    public void UpdateUI(float temperature)
    {
        fill.fillAmount = temperature.Remap(36.5f, 38.5f, fillMinValue, 1);
        indicatorDigital.text = $"{temperature}°C";
    }

}
