﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moth : GenericEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        damageAmount = 1;
        maxHealth = 1;
        moveRate = 5f;
        maxRange = 20;
        minRange = 4;
        backupDistance = 1;
        backupRate = 2;
        attackDistance = 5;
        attackRate = 25;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            handleDeath();

        }
    }

    private void handleDeath()
    {
        Destroy(this.gameObject);
    }
}
