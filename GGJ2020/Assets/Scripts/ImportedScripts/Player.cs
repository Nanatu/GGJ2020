﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{

    //Inputs
    Controller2D controller;
    Vector2 directionalInput;
    PolygonCollider2D lightAttackHurtBox;
    PolygonCollider2D heavyAttackHurtBox;
    PolygonCollider2D downAttackHurtBox;
    public Transform ColliderTransform;

    public Animator animator;

    public AnimationVerbs animVerbs;

    //Movement Variables
    float gravity;
    Vector3 velocity;
    float moveSpeed = 6;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;

    //Jump Variables
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float maxJumpVelocity;
    float minJumpVelocity;
    float accelerationTimeAirborne = .2f;

    //Attack Variables
    int comboPresses = 0;
    float lastAttackPressTime = 0;
    float maxComboDelay = 1.0f;
    public bool attackInput;

    //WallCling Variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    bool wallSliding;
    int wallDirX;
    float timeToWallUnstick;


    void Start()
    {
        controller = GetComponent<Controller2D>();

        //Get Attack type children
        lightAttackHurtBox = this.transform.GetChild(0).GetComponent<PolygonCollider2D>();
        lightAttackHurtBox.enabled = false;

        heavyAttackHurtBox = this.transform.GetChild(1).GetComponent<PolygonCollider2D>();
        heavyAttackHurtBox.enabled = false;

        downAttackHurtBox = this.transform.GetChild(2).GetComponent<PolygonCollider2D>();
        downAttackHurtBox.enabled = false;

        //Derive Gravity and Jump heights
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }

        if (Time.time - lastAttackPressTime > maxComboDelay)
        {
            comboPresses = 0;
        }


        if (attackInput && (directionalInput.y == -1.0f) && !controller.collisions.below)
        {
            animator.SetTrigger("DownAttackT");
        }
        else if (attackInput)
        {
            lastAttackPressTime = Time.time;
            comboPresses++;
            if (comboPresses == 1)
            {
                //animator.SetBool("LightAttack", true);
                animator.SetTrigger("LightAttackT");
            }
            Debug.Log("Combo Presses = " + comboPresses);
        }

    }

    public void ReturnLightAttack()
    {
        if (comboPresses >= 2)
        {
            //animator.SetBool("HeavyAttack", true);
            animator.SetTrigger("HeavyAttackT");
        }
        else
        {
           //animator.SetBool("LightAttack", false);
            comboPresses = 0;
            lightAttackHurtBox.gameObject.SendMessage("ResetCollisionList");
        }
    }

    public void ReturnHeavyAttack()
    {
        //animator.SetBool("LightAttack", false);
       // animator.SetBool("HeavyAttack", false);
        comboPresses = 0;
        lightAttackHurtBox.gameObject.SendMessage("ResetCollisionList");
        heavyAttackHurtBox.gameObject.SendMessage("ResetCollisionList");
    }

    public void ReturnDownAttack()
    {
        downAttackHurtBox.gameObject.SendMessage("ResetCollisionList");
    }


    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }


    public void OnLightAttackDown()
    {
        animator.SetTrigger("LightAttackT");
    }

    public void OnHeavyAttackDown()
    {
        animator.SetTrigger("HeavyAttackT");
    }

    public void EnableLightHurtBox()
    {
        lightAttackHurtBox.enabled = true;
    }

    public void EnableHeavytHurtBox()
    {
        heavyAttackHurtBox.enabled = true;
    }

    public void EnableDowntHurtBox()
    {
        downAttackHurtBox.enabled = true;
    }

    public void IncrememntCombo()
    {
        comboPresses++;
    }

    public void DecrementCombo()
    {
        comboPresses--;
    }

    public void DisableHurtBox()
    {
        //animator.SetBool("LightAttack", false);
        //animator.SetBool("HeavyAttack", false);
        lightAttackHurtBox.enabled = false;
        heavyAttackHurtBox.enabled = false;
        downAttackHurtBox.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        //TODO
    }

    //Jump button pressed
    public void OnJumpInputDown()
    {
        animator.SetTrigger("LeapT");
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                animator.ResetTrigger("LeapT");
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
                transform.localScale = new Vector2(Mathf.Sign(-wallDirX), transform.localScale.y);
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                // Disallow wall jumps against max slope
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    //Jump button released
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            transform.localScale = new Vector2(Mathf.Sign(wallDirX), transform.localScale.y);

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        if (velocity.y > 0 )
        {
            animator.SetBool("Rising", true);
            animator.SetBool("Falling", false);
           
        }
        else if (velocity.y < 0 && !controller.collisions.below)
        {
            animator.SetBool("Falling", true);
            animator.SetBool("Rising", false);
            animator.ResetTrigger("LeapT");
        }
        else
        {
            animator.SetBool("Falling", false);
            animator.SetBool("Rising", false);
        }

        if (controller.collisions.left || controller.collisions.right)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        }
    }


    public struct AnimationVerbs
    {
        public bool attacking;
        public bool rising;
        public bool falling;

        public void Reset()
        {
            attacking = false;
            rising = false;
            falling = false;
        }
    }
}