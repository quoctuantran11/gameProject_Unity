using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager
{
    private static EnemyStatsManager _instance;

    public static EnemyStatsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyStatsManager();
            }
            return _instance;
        }
    }

    private int baseHealth = 100;
    private int healthIncrease = 20;
    private int baseSpeed = 2;
    private int speedIncrease = 1;
    private int baseAttackDamage = 10;
    private int attackDamageIncrease = 1;
    private int baseAttackRate = 1;
    private int attackRateIncrease = 1;
    private int mobilityUpgradePeriod = 3;


    public EnemyStats enemyStatsAtLevel(int level){
        if (level > 1){
            int health = this.baseHealth + healthIncrease * level;
            int speed = this.baseSpeed + speedIncrease * (level / mobilityUpgradePeriod);
            int damage = this.baseAttackDamage + attackDamageIncrease * level;
            int rate = this.baseAttackRate + attackRateIncrease * (level / mobilityUpgradePeriod);
            return new EnemyStats(health, speed, damage, rate);
        }
        else{
            return new EnemyStats();
        }
    }

    public EnemyStats HighDamageEnemyStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        return new EnemyStats(baseEnemyStats.maxHealth - 30, baseEnemyStats.speed, baseEnemyStats.attackDamage + 4, baseEnemyStats.attackRate);
    }

    public EnemyStats HighHealthEnemyStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        return new EnemyStats(baseEnemyStats.maxHealth + 40, baseEnemyStats.speed - 1, baseEnemyStats.attackDamage - 3, baseEnemyStats.attackRate);
    }

    public EnemyStats HighMobilityEnemyStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        return new EnemyStats(baseEnemyStats.maxHealth - 40, baseEnemyStats.speed + 2, baseEnemyStats.attackDamage - 3, baseEnemyStats.attackRate);
    }

    public EnemyStats bossStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        int health = baseEnemyStats.maxHealth * 2;
        int damage = baseEnemyStats.attackDamage + 5 * level;
        int attackRate = 1;
        int speed = 1;
        return new EnemyStats(health, speed, damage, attackRate);
    }

}
