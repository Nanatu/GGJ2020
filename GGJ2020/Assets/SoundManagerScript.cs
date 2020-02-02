using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip playerAttackSound, playerDeathSound, playerJumpSound, enemyDeathSound, dustAttack, jackedRabbitEnter;

    static AudioSource audioSrc;
    
    // Start is called before the first frame update
 
    void Start()
    {

        playerAttackSound = Resources.Load<AudioClip> ("mubAttack");
        playerDeathSound = Resources.Load<AudioClip> ("mub_death1");
        playerJumpSound = Resources.Load<AudioClip> ("mubJump");
        enemyDeathSound = Resources.Load<AudioClip> ("dustDeath");
        dustAttack = Resources.Load<AudioClip> ("dustbunny_attack3");
        jackedRabbitEnter = Resources.Load<AudioClip> ("jackedRabbit");

        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "mubAttack":
                audioSrc.PlayOneShot(playerAttackSound);
                break;
            case "mub_death1":
                audioSrc.PlayOneShot(playerDeathSound);
                break;
            case "mubJump":
                audioSrc.PlayOneShot(playerJumpSound);
                break;
            case "dustDeath":
                audioSrc.PlayOneShot(enemyDeathSound);
                break;
            case "dustbunny_attack3":
                audioSrc.PlayOneShot(dustAttack);
                break;
            case "jackedRabbit":
                audioSrc.PlayOneShot(jackedRabbitEnter);
                break;

        }
    }

}
