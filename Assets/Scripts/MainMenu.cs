using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowOptionMenu()
    {
        Debug.Log("Option Menu");
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
    }
}
