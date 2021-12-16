using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager
{
    // Singleton

    private static MyGameManager _instance;

    public static MyGameManager Instance
    {
        get
        {
            if(_instance == null)
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
}
