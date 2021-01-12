using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] Menu[] menus;
    public string lvlToLoad;

    public MenuSounds sfx;

    public void StartGame(string soundToCall)
    {
        StartCoroutine(StartGameRoutine(soundToCall));
    }

    public void ExitGame(string soundToCall)
    {
        StartCoroutine(ExitGameRoutine(soundToCall));
    }

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator StartGameRoutine(string soundToCall)
    {
        sfx.CallSound(soundToCall);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(lvlToLoad);
    }

    IEnumerator ExitGameRoutine(string soundToCall)
    {
        sfx.CallSound(soundToCall);
        yield return new WaitForSeconds(2);
        Application.Quit();
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
