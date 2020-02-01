using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBunny : GenericEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        damageAmount = 10;
        maxHealth = 75;
        moveRate = 7.5f;
        maxRange = 15;
        minRange = 4;
        backupDistance = 1;
        backupRate = 1;
        attackDistance = 5;
        attackRate = 25;
        base.Start();
}

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
