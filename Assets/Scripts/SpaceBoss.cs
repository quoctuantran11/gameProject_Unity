using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceBoss : EnemyController
{
    private MusicController music;

    void Awake() {
        music = FindObjectOfType<MusicController>();
    }

    void BossDefeated()
    {
        music.PlaySong(music.levelClearSong);
        FindObjectOfType<UIManager>().UpdateDisplayMessage("Level clear");
        //Invoke("LoadScene", 8f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void randomEnemyType(){
        this.setStats(EnemyStatsManager.Instance.bossStatsAtLevel(1));
    }
}
