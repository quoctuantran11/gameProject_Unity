using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject maps, modes, spaceLevels, streetLevels, optionsMenu, Cam;
    bool space, street, option;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMap()
    {
        maps.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void setBalance()
    {
        MyGameManager.Instance.getType = 1;
        modes.SetActive(false);
    }

    public void setAggressive()
    {
        MyGameManager.Instance.getType = 2;
        modes.SetActive(false);
    }

    public void setEndurance()
    {
        MyGameManager.Instance.getType = 3;
        modes.SetActive(false);
    }

    public void ShowSpaceLevels()
    {
        modes.SetActive(true);
        spaceLevels.SetActive(true);
        space = true;
    }

    public void ShowStreetLevels()
    {
        modes.SetActive(true);
        streetLevels.SetActive(true);
        street = true;
    }

    public void PlayGame(string sceneName)
    {
        MyGameManager.Instance.getLevel = Int32.Parse(EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMP_Text>().text);
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.SetInt("playerStat", MyGameManager.Instance.getType);
    }

    public void ShowOptionMenu()
    {
        optionsMenu.SetActive(true);
        option = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void PauseGame()
    {
        MyGameManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        MyGameManager.Instance.ResumeGame();
        this.gameObject.SetActive(false);
    }

    public void LoadMainMenu()
    {
        //optionsMenu.SetActive(false);
        MyGameManager.Instance.LoadMainMenu();
        SceneManager.LoadScene("Menu");
    }

    public void Back()
    {
        if(space)
        {
            spaceLevels.SetActive(false);
            space = false;
            maps.SetActive(true);
        }

        if(street)
        {
            streetLevels.SetActive(false);
            street = false;
            maps.SetActive(true);
        }

        if(option)
        {
            optionsMenu.SetActive(false);
            option = false;
        }
    }
}
