using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MyGameManager
{
    public bool isNewGame = true;
    // Singleton
    private static MyGameManager _instance;

    public static MyGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MyGameManager();
            }
            return _instance;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    //To main menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
    }
}
