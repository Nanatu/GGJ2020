using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : GenericEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 50;
        damageAmount = 5;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
