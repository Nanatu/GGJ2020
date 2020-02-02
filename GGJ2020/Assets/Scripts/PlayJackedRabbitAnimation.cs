using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayJackedRabbitAnimation : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManagerScript.PlaySound("jackedRabbit");
            myAnimationController.SetBool("JackedRabbitIsReady", true);
        }
    }
}
