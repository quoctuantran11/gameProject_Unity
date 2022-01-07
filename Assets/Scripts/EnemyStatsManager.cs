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
    private int healthIncrease = 30;
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
        return new EnemyStats(baseEnemyStats.maxHealth - 30, baseEnemyStats.speed, baseEnemyStats.attackDamage + 3 * level, baseEnemyStats.attackRate + 1);
    }

    public EnemyStats HighHealthEnemyStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        return new EnemyStats(baseEnemyStats.maxHealth + 35 * level, baseEnemyStats.speed - 1, baseEnemyStats.attackDamage - 2, baseEnemyStats.attackRate);
    }

    public EnemyStats HighMobilityEnemyStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        return new EnemyStats(baseEnemyStats.maxHealth - 40, baseEnemyStats.speed + 2, baseEnemyStats.attackDamage - 3, baseEnemyStats.attackRate + (level / 2));
    }

    public EnemyStats bossStatsAtLevel(int level){
        EnemyStats baseEnemyStats = this.enemyStatsAtLevel(level);
        int health = baseEnemyStats.maxHealth * 2 + 50 * level;
        int damage = baseEnemyStats.attackDamage + 5 * level;
        int attackRate = 2;
        int speed = 2;
        return new EnemyStats(health, speed, damage, attackRate);
    }

}
