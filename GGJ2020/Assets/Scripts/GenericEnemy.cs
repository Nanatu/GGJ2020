using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEnemy : MonoBehaviour
{

    List<GameObject> currentCollisions = new List<GameObject>();

    public float damageAmount = 10f;    // default value
    public float health = 100f;        // default value
    public float maxHealth = 100f;      // default value
    public float moveRate = 10f;        // default value
    public float maxRange = 10f;        // default value
    public float minRange = 1f;         // default value
    private float distanceToPlayer = 1000f;
    public float attackRate = 2f;
    private bool attacking = false;
    public float attackDistance = 4f;
    public float attackCooldown = 3f;

    private GameObject Player;
    public Animator anim;

    // Start is called before the first frame update
    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        moveToEnemy();
        //print(distanceToPlayer);
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
        Destroy(this);
    }

    private void moveToEnemy()
    {
        if (distanceToPlayer < maxRange && distanceToPlayer > minRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveRate * Time.deltaTime);
            print(transform.position);
        }
        if (distanceToPlayer < minRange)
        {
            if (!attacking)
            {
                attack();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && !currentCollisions.Contains(col.gameObject))
            {
                currentCollisions.Add(col.gameObject);
                col.gameObject.SendMessage("TakeDamage", damageAmount);
                Debug.Log("Adding" + col.gameObject.tag + "to List");
            }
    }
    

    public void attack()
    {
        attacking = true;
        var x_diff = Player.transform.position.x - transform.position.x;
        var attackDirection = 1;
        if (x_diff < 0)
        {
            attackDirection = -1;
        }
        Vector3 attackTarget = transform.position + new Vector3(attackDistance * attackDirection, 0,0);
        while (transform.position != attackTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackTarget, attackRate * Time.deltaTime);
        }
        StartCoroutine(waiter());
        
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }
}
