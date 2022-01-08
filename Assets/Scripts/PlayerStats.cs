using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public int maxHealth;
    public int speed;
    public int maxSpeed;
    public int attackDamage;
    public float attackRate;
    public int defense = 0;
    public int lifeSteal = 0;

    public PlayerStats(int health, int speed, int maxSpeed, int attack, float rate, int defense, int lifeSteal){
        this.maxHealth = health;
        this.speed = speed;
        this.maxSpeed = maxSpeed;
        this.attackDamage = attack;
        this.attackRate = rate;
        this.defense = defense;
        this.lifeSteal = lifeSteal;
    }

    public PlayerStats(){
        this.maxHealth = 100;
        this.speed = 3;
        this.maxSpeed = 5;
        this.attackDamage = 10;
        this.attackRate = 1f;
        this.defense = 20;
        this.lifeSteal = 10;
    }
}
