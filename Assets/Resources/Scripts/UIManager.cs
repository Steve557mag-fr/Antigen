using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("_References_")]
    public UIThermometer thermometer;
    public CanvasGroup groupGameOver, groupWin, groupAlert;
    public TextMeshProUGUI textCurrentBacteriaKilled, textCurrentBacteria;

    internal void UpdateScores(int currentBacteria, int maxBacteria)
    {
        textCurrentBacteria.text = currentBacteria.ToString("000");
        textCurrentBacteriaKilled.text = (maxBacteria - currentBacteria).ToString("000");
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

    internal void DisplayAlert()
    {
        groupAlert.gameObject.SetActive(true);
        groupAlert.LeanAlpha(1, 0.5f);
    }

    internal void ResetAlert()
    {
        groupAlert.alpha = 0;
        groupAlert.gameObject.SetActive(false);
    }

    public void SpawnAlly(int id)
    {
        GameLoop.GetGameLoop().SpawnAllyFromPanel(id);
    }

}
