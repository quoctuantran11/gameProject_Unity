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
        Debug.Log(MyGameManager.Instance.getLevel);
    }

    void BossDefeated()
    {
        music.PlaySong(music.levelClearSong);
        FindObjectOfType<UIManager>().UpdateDisplayMessage("Level clear");
        PlayerPrefs.SetInt("level", ++MyGameManager.Instance.getLevel);
        MyGameManager.Instance.getQuantity += MyGameManager.Instance.getQuantity + (MyGameManager.Instance.getLevel/2);
        Invoke("LoadScene", 6f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("SpaceMap");
    }

    public override void randomEnemyType(){
        this.setStats(EnemyStatsManager.Instance.bossStatsAtLevel(MyGameManager.Instance.getLevel));
        if(MyGameManager.Instance.getLevel % 5 == 0)
        {
            sprite.color = new Color(0, 0.588f, 0.862f);
        }
    }
}
