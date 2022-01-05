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

    public PlayerStats(int health, int speed, int maxSpeed, int attack, float rate){
        this.maxHealth = health;
        this.speed = speed;
        this.maxSpeed = maxSpeed;
        this.attackDamage = attack;
        this.attackRate = rate;
    }

    public PlayerStats(){
    this.maxHealth = 100;
    this.speed = 3;
    this.maxSpeed = 5;
    this.attackDamage = 10;
    this.attackRate = 1f;
    }
}
