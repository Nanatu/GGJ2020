using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip playerAttackSound, playerDeathSound, playerJumpSound, enemyDeathSound;

    static AudioSource audioSrc;
    
    // Start is called before the first frame update
 
    void Start()
    {

        playerAttackSound = Resources.Load<AudioClip> ("mubAttack");
        playerDeathSound = Resources.Load<AudioClip> ("mubDeath");
        playerJumpSound = Resources.Load<AudioClip> ("mubJump");
        enemyDeathSound = Resources.Load<AudioClip> ("dustDeath");

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
            case "mubDeath":
                audioSrc.PlayOneShot(playerDeathSound);
                break;
            case "mubJump":
                audioSrc.PlayOneShot(playerJumpSound);
                break;
            case "dustDeath":
                audioSrc.PlayOneShot(enemyDeathSound);
                break;

        }
    }

}
