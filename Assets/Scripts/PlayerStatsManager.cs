using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager
{
    
    private int playerMode;     //1: Balance - 2: Aggressive - 3: Endurance
    private int baseHealth;
    private int healthIncrease;
    private int baseSpeed;
    private int baseMaxSpeed;
    private int speedIncrease;
    private int baseAttackDamage;
    private int attackDamageIncrease;
    private float baseAttackRate;
    private float attackRateIncrease;
    private int speedUpgradePeriod;


    public PlayerStatsManager(int mode){
        this.playerMode = mode;
        switch(mode){
            case 2:
                this.initAggressiveStats();
                break;
            case 3:
                this.initEnduranceStats();
                break;
            default:
                this.initBalanceStats();
                break;
        }
    }

    private void initBalanceStats(){
        this.baseHealth = 100;
        this.healthIncrease = 30;
        this.baseSpeed = 3;
        this.baseMaxSpeed = 5;
        this.speedIncrease = 1;
        this.baseAttackDamage = 10;
        this.attackDamageIncrease = 2;
        this.baseAttackRate = 1f;
        this.attackRateIncrease = 0.05f;
        this.speedUpgradePeriod = 3;
    }

    private void initAggressiveStats(){
        this.baseHealth = 80;
        this.healthIncrease = 15;
        this.baseSpeed = 4;
        this.baseMaxSpeed = 6;
        this.speedIncrease = 1;
        this.baseAttackDamage = 12;
        this.attackDamageIncrease = 3;
        this.baseAttackRate = 1.1f;
        this.attackRateIncrease = 0.1f;
        this.speedUpgradePeriod = 3;
    }

    private void initEnduranceStats(){
        this.baseHealth = 120;
        this.healthIncrease = 40;
        this.baseSpeed = 2;
        this.baseMaxSpeed = 4;
        this.speedIncrease = 1;
        this.baseAttackDamage = 10;
        this.attackDamageIncrease = 2;
        this.baseAttackRate = 1f;
        this.attackRateIncrease = 0.05f;
        this.speedUpgradePeriod = 3;
    }

    public PlayerStats playerStatsAtLevel(int level){
        level = level - 1;
        int health = this.baseHealth + level * healthIncrease;
        int speed = this.baseSpeed + (level / speedUpgradePeriod) * this.speedIncrease;
        int maxSpeed = this.baseMaxSpeed + (level / speedUpgradePeriod) * this.speedIncrease;
        int damage = this.baseAttackDamage + this.attackDamageIncrease * level;
        float rate = this.baseAttackRate + this.attackRateIncrease * level;

        return new PlayerStats(health, speed, maxSpeed, damage, rate);
    }
}
