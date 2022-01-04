using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class MyGameManager
{
    public string fileName;
    public bool isNewGame = true;
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

    public void SaveGame(GameObject hero, GameObject enemySpawn, GameObject troop, GameObject leader)
    {
        string path = Path.Combine(Application.persistentDataPath, "savedFile.json");
        //Create data
        Player player = hero.GetComponent<Player>();
        EnemySpawn spawn = enemySpawn.GetComponent<EnemySpawn>();
        EnemyController enemy = troop.GetComponent<EnemyController>();
        Boss boss = leader.GetComponent<Boss>();
        SavedData savedData = new SavedData(player.health, boss.health, spawn.enemyCurs,
                                            enemy.attackDamage, boss.attackDamage,
                                            player.transform.position, boss.transform.position,
                                            player.playerName);
        //Create binary formatter
        string json = JsonUtility.ToJson(savedData);
        File.WriteAllText(path, json);
        Debug.Log("Game saved " + path);
    }

    public void LoadGame(Player player, EnemySpawn spawn, Boss boss)
    {
        string path = Path.Combine(Application.persistentDataPath, "savedFile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedData savedData = (SavedData)JsonUtility.FromJson<SavedData>(json);
            //Load data
            player.health = savedData.playerHealth;
            player.transform.position = new Vector2(savedData.playerPosition[0],
                                                    savedData.playerPosition[1]);
            player.playerName = savedData.playerName;

            boss.health = savedData.bossHealth;
            boss.transform.position = new Vector2(savedData.bossPosition[0],
                                                    savedData.bossPosition[1]);                                        
            boss.attackDamage = savedData.bossDamage;

            spawn.enemyCurs = savedData.numberOfEnemy;
            Debug.Log("Game loaded " + path);
        }
        else
        {
            Debug.Log("Saved file does not exist!");
        }
    }
}
