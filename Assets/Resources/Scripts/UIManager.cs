using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("_References_")]
    public UIThermometer thermometer;
    public CanvasGroup groupGameOver, groupWin;
    public TextMeshProUGUI textCurrentBacteriaKilled, textCurrentBacteria;

    internal void UpdateScores(int currentBacteria, int maxBacteria)
    {

        textCurrentBacteria.text = currentBacteria.ToString();
        textCurrentBacteriaKilled.text = (maxBacteria - currentBacteria).ToString();

    }

    internal void DisplayGameOver()
    {
        groupGameOver.gameObject.SetActive(true);
        groupGameOver.LeanAlpha(1, 1f);
    }

    internal void DisplayWin()
    {
        groupWin.gameObject.SetActive(true);
        groupWin.LeanAlpha(1, 1f);
    }

}
