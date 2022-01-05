using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        modes.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ShowSpaceLevels()
    {
        spaceLevels.SetActive(true);
        space = true;
    }

    public void ShowStreetLevels()
    {
        streetLevels.SetActive(true);
        street = true;
    }

    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
