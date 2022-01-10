using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MyGameManager
{
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

    int defaultLevel;

    public int getLevel
    {
        get {return defaultLevel; }
        set
        {
            defaultLevel = value;
        }
    }

    int playerType;

    public int getType
    {
        get {return playerType; }
        set
        {
            playerType = value;
        }
    }

    int enemyQuantity = 4;

    public int getQuantity
    {
        get {return enemyQuantity;}
        set
        {
            enemyQuantity = value;
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
