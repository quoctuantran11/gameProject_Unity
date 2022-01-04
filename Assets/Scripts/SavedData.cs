using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SavedData
{
    public int playerHealth, bossHealth, numberOfEnemy, enemyDamage, bossDamage;
    public float[] playerPosition, bossPosition;
    public string playerName;

    public SavedData(int playerHealth, int bossHealth, int numberOfEnemy, int enemyDamage,
                    int bossDamage, Vector2 playerPosition, Vector2 bossPosition, string playerName)
    {
        this.playerHealth = playerHealth;
        this.bossHealth = bossHealth;
        this.numberOfEnemy = numberOfEnemy;
        this.enemyDamage = enemyDamage;
        this.bossDamage = bossDamage;
        this.playerName = playerName;

        playerPosition[0] = playerPosition.x;
        playerPosition[1] = playerPosition.y;

        bossPosition[0] = bossPosition.x;
        bossPosition[1] = bossPosition.y;
    }
}
