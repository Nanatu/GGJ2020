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

    public GameObject Player;
    public Animator anim;

    // Start is called before the first frame update
    public void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        moveToEnemy();
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
            transform.LookAt(Player.transform);
            transform.position += transform.forward * moveRate * Time.deltaTime;
        }
        if (distanceToPlayer < minRange)
        {
            attack();
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
    

    private void attack()
    {
        
    }
}
