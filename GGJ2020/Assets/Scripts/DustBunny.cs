using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class DustBunny : MonoBehaviour
{
    List<GameObject> currentCollisions = new List<GameObject>();

    public float targetTime = 2.0f;

    public float damageAmount = 1f;    // default value
    public Animator animator;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 4;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    public bool attackInput;
    int comboPresses = 0;

    public GameObject target;

    public int health = 3;

    public bool canAttack = true;

    private bool attacking = false;
    public float attackCooldown = 3f;

    void Start()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        controller = GetComponent<Controller2D>();

        target = GameObject.Find("Mub");

    }
    void Update()


    {
        //if (!canAttack)
        //{
        //    targetTime -= Time.deltaTime;

        //    if (targetTime <= 0.0f)
        //    {
        //        timerEnded();
        //    }
        //}
        if (Vector2.Distance(transform.position, target.transform.position) <= 5)
        {
            if (!attacking)
            {
                attacking = true;
                animator.SetTrigger("RevealT");
                animator.SetBool("Chase", true);
            }

            if (target.transform.position.x > transform.position.x)
            {
                directionalInput = Vector2.right;
            }
            else
            {
                directionalInput = Vector2.left;
            }
        }
        else
        {
            attacking = false;
            animator.ResetTrigger("RevealT");
            animator.SetBool("Chase", false);
            directionalInput = Vector2.zero;
        }

        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_LightAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_HeavyAttack")
        //    || animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Hit"))
        //{
        //    directionalInput = Vector2.zero;
        //}

        CalculateVelocity();

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


        //if (Vector2.Distance(transform.position, target.transform.position) <= 3)
        //{
        //    //lastAttackPressTime = Time.time;
        //    comboPresses++;
        //    if (comboPresses == 1)
        //    {
        //        //animator.SetBool("LightAttack", true);
        //        animator.SetTrigger("LightAttackT");
        //    }
        //    // Debug.Log("Enemy Combo Presses = " + comboPresses);
        //    comboPresses = Mathf.Clamp(comboPresses, 0, 2);
        //}
    }

    //public void ReturnLightAttack()
    //{
    //    if (comboPresses >= 2)
    //    {
    //        //animator.SetBool("HeavyAttack", true);
    //        animator.SetTrigger("HeavyAttackT");
    //    }
    //    else
    //    {
    //        animator.SetBool("LightAttack", false);
    //        comboPresses = 0;
    //    }
    //}

    //public void ReturnHeavyAttack()
    //{
    //    //animator.SetBool("LightAttack", false);
    //    //animator.SetBool("HeavyAttack", false);
    //    comboPresses = 0;
    //    directionalInput = Vector2.zero;
    //}
    //public void DisableHurtBox()
    //{
    //    //animator.SetBool("LightAttack", false);
    //    //animator.SetBool("HeavyAttack", false);
    //    // lightAttackHurtBox.enabled = false;
    //    // heavyAttackHurtBox.enabled = false;
    //}

    void ResetDamaged()
    {
        Debug.Log("Set Damaged to False");
        animator.SetBool("Damaged", false);
    }

    //void timerEnded()
    //{
    //    canAttack = true;
    //    currentCollisions.Clear();
    //}


    void OnTriggerStay2D(Collider2D col)
    {
        //if (canAttack)
        //{

        //if (!attacking)
        //{
        //    attacking = true;
        //    animator.settrigger("revealt");
        //    animator.setbool("chase", true);
        //}

        if (col.gameObject.tag == "Player" && !currentCollisions.Contains(col.gameObject) && attacking == true)
        {
            currentCollisions.Add(col.gameObject);
            col.gameObject.SendMessage("TakeDamage", damageAmount);
            print(damageAmount);
            //canAttack = false;
            //targetTime = 2.0f;
            animator.SetBool("Chase", false);
            attacking = false;
            Debug.Log("Adding" + col.gameObject.tag + "to List");

            StartCoroutine(attackCooldownTime());
        }
            //}
  
    }

    void ResetCollisionList()
    {
        Debug.Log("Clearing Collision List");
        currentCollisions.Clear();
    }

    IEnumerator attackCooldownTime()
    {
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        ResetCollisionList();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Taking" + damage + "Damage!");
        animator.SetBool("Damaged", true);
        health = health - damage;
        Debug.Log("Enemy Health = " + health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void HitMove(Vector2 move)
    {
        velocity.x = move.x;
        velocity.y = move.y;
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;


        if (controller.collisions.left || controller.collisions.right)
        {
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        }
    }
}
