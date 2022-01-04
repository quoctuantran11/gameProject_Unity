using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject maps, spaceLevels, optionsMenu, Cam, player, boss, spawn, enemy;
    bool space, street;
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

    public void ShowSpaceLevels()
    {
        spaceLevels.SetActive(true);
        space = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SpaceMap");
    }

    public void ShowOptionMenu()
    {
        optionsMenu.SetActive(true);
        maps.SetActive(false);
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
        optionsMenu.SetActive(false);
        MyGameManager.Instance.LoadMainMenu();
        SceneManager.LoadScene("Menu");
    }

    public void SaveGame()
    {
        MyGameManager.Instance.SaveGame(player, spawn, enemy, boss);
    }

    public void LoadGame()
    {
        MyGameManager.Instance.isNewGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        if(space)
        {
            spaceLevels.SetActive(false);
        }
        
        maps.SetActive(true);
    }
}
