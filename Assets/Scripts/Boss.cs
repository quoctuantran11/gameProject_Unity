using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : EnemyController
{
    public GameObject boomerang;
    public float minTime, maxTime;
    private MusicController music;

    void Awake() {
        Invoke("ThrowBoomerang", Random.Range(minTime, maxTime));
        music = FindObjectOfType<MusicController>();
        music.PlaySong(music.bossSong);
    }

    void ThrowBoomerang()
    {
        animator.SetTrigger("Boomerang");
        GameObject tempBoomerang = Instantiate(boomerang, transform.position, transform.rotation);
        if(isFlipped)
        {
            tempBoomerang.GetComponent<Boomerang>().direction = 1;
        }
        else
        {
            tempBoomerang.GetComponent<Boomerang>().direction = -1;
        }
        Invoke("ThrowBoomerang", Random.Range(minTime, maxTime));
    }

    void BossDefeated()
    {
        music.PlaySong(music.levelClearSong);
        FindObjectOfType<UIManager>().UpdateDisplayMessage("Level clear");
        Invoke("LoadScene", 6f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
