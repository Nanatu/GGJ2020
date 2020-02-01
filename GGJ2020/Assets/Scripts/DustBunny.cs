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
        moveRate = 5;
        maxRange = 15;
        minRange = 3;
        base.Start();
}

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
