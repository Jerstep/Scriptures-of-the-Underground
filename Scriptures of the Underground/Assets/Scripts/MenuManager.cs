using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject controlsScreen;
    public string lvlToLoad;
    bool controlsScreenActive;

    public void StartGame()
    {
        SceneManager.LoadScene(lvlToLoad);
    }

    public void ControlScreenToggle()
    {
        controlsScreenActive = !controlsScreenActive;
        controlsScreen.SetActive(controlsScreenActive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
