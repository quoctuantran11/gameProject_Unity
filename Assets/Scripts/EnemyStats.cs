using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats{
    public int maxHealth = 100;
    public int speed = 2;
    public int attackDamage = 10;
    public float attackRange = 2f;
    public int attackRate = 1;

    public EnemyStats(int health, int speed, int damage, int rate){
        this.maxHealth = health;
        this.speed = speed;
        this.attackDamage = damage;
        this.attackRate = rate;
    }
    public EnemyStats(){}
}
