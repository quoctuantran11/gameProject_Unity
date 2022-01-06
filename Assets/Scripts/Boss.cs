using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : EnemyController
{
    private MusicController music;

    void Awake() {
        music = FindObjectOfType<MusicController>();
        music.PlaySong(music.bossSong);
    }

    void BossDefeated()
    {
        music.PlaySong(music.levelClearSong);
        FindObjectOfType<UIManager>().UpdateDisplayMessage("Level clear");
        PlayerPrefs.SetInt("level", ++FindObjectOfType<GameManager>().defaultLevel);
        Invoke("LoadScene", 6f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void randomEnemyType(){
        this.setStats(EnemyStatsManager.Instance.bossStatsAtLevel(FindObjectOfType<GameManager>().defaultLevel));
    }
}
