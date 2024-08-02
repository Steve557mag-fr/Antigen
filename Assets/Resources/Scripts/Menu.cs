using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("GameLoop");
    }

    public void Quit()
    {
        Application.Quit();
    }



}
