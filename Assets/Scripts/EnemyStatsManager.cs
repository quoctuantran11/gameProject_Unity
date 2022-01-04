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

    public int baseHealth = 100;
    public int healthIncrease = 20;
    public int baseSpeed = 2;
    public int speedIncrease = 1;
    public int baseAttackDamage = 10;
    public int attackDamageIncrease = 1;
    public int baseAttackRate = 1;
    public int attackRateIncrease = 1;
    public int mobilityUpgradePeriod = 3;


}
